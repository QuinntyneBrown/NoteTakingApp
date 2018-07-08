using Microsoft.AspNetCore.SignalR.Client;
using NoteTakingApp.API.Features.Notes;
using NoteTakingApp.API.Features.Tags;
using NoteTakingApp.Core.Extensions;
using NoteTakingApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class NoteScenarios: NoteScenarioBase
    {
        [Fact]
        public async Task ShouldDeleteNote()
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (var server = CreateServer())
            {
                var context = server.Host.Services.GetService(typeof(AppDbContext)) as AppDbContext;

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveNoteCommand.Request,SaveNoteCommand.Response>(Post.Notes, new SaveNoteCommand.Request
                    {
                        Note = new NoteApiModel
                        {
                            Title = "Title",
                            Body = "Body",
                        }
                    });

                context.SaveChanges();

                var hubConnection = GetHubConnection(server.CreateHandler());

                hubConnection.On<dynamic>("message", (result) =>
                {
                    Assert.Equal("[Note] Removed", $"{result.type}");
                    Assert.Equal(saveResponse.NoteId, Convert.ToInt16(result.payload.noteId));
                    tcs.SetResult(true);
                });

                await hubConnection.StartAsync();

                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Note(saveResponse.NoteId, 0));

                response.EnsureSuccessStatusCode();

                await tcs.Task;
            }
        }
        [Fact]
        public async Task ShouldSaveNote()
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (var server = CreateServer())
            {
                var hubConnection = GetHubConnection(server.CreateHandler());

                hubConnection.On<dynamic>("message", (result) =>
                {
                    Assert.Equal("[Note] Saved", $"{result.type}");
                    Assert.Equal(1, Convert.ToInt16(result.payload.note.noteId));
                    tcs.SetResult(true);
                });

                await hubConnection.StartAsync();

                var response = await server.CreateClient()
                    .PostAsAsync<SaveNoteCommand.Request, SaveNoteCommand.Response>(Post.Notes, new SaveNoteCommand.Request() {
                        Note = new NoteApiModel()
                        {
                            Title = "First Note",
                            Body = "<p>Something Important</p>",
                            Tags = new List<TagApiModel>() { new TagApiModel() { TagId = 1, Name = "Angular" } }
                        }
                    });

                Assert.True(response.NoteId == 1);    
            }

            await tcs.Task;
        }

        [Fact]
        public async Task ShouldGetAllNotes()
        {
            using (var server = CreateServer())
            {
                await server.CreateClient()
                    .PostAsync(Post.Notes, new 
                    {
                        Note = new
                        {
                            Title = "First Note",
                            Body = "<p>Something Important</p>",
                        }
                    });

                var response = await server.CreateClient()
                    .GetAsync<GetNotesQuery.Response>(Get.Notes);

                Assert.True(response.Notes.Count() == 1);
            }
        }

        [Fact]
        public async Task ShouldGetNoteById()
        {
            using (var server = CreateServer())
            {
                await server.CreateClient()
                    .PostAsync(Post.Notes, new
                    {
                        Note = new
                        {
                            Title = "First Note",
                            Body = "<p>Something Important</p>",
                        }
                    });
                
                var response = await server.CreateClient()
                    .GetAsync<GetNoteByIdQuery.Response>(Get.NoteById(1));

                Assert.True(response.Note.NoteId != default(int));
            }
        }

        [Fact]
        public async Task ShouldGetNoteBySlug()
        {            
            using (var server = CreateServer())
            {
                await server.CreateClient()
                    .PostAsync(Post.Notes, new
                    {
                        Note = new
                        {
                            Title = "Title",
                            Body = "<p>Something Important</p>",
                        }
                    });
                
                var response = await server.CreateClient()
                    .GetAsync<GetNoteBySlugQuery.Response>(Get.NoteBySlug("title"));

                Assert.True(response.Note.NoteId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdateNote()
        {
            using (var server = CreateServer())
            {
                await server.CreateClient()
                    .PostAsync(Post.Notes, new
                    {
                        Note = new
                        {
                            Title = "Title",
                            Body = "<p>Something Important</p>",
                        }
                    });

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveNoteCommand.Request, SaveNoteCommand.Response>(Post.Notes, new SaveNoteCommand.Request()
                    {
                        Note = new NoteApiModel()
                        {
                            NoteId = 1,
                            Title = "Title",
                            Body = "Body"
                        }
                    });

                Assert.True(saveResponse.NoteId == 1);
            }
        }

        [Fact]
        public async Task ShouldFailVersion()
        {
            using (var server = CreateServer())
            {
                await server.CreateClient()
                    .PostAsync(Post.Notes, new
                    {
                        Note = new
                        {
                            Title = "Title",
                            Body = "<p>Something Important</p>",
                            Version = ""
                        }
                    });

                var x = await server.CreateClient()
                    .GetAsync<GetNoteByIdQuery.Response>(Get.NoteById(1));

                await server.CreateClient()
                    .PostAsAsync<SaveNoteCommand.Request, SaveNoteCommand.Response>(Post.Notes, new SaveNoteCommand.Request()
                    {
                        Note = new NoteApiModel()
                        {
                            NoteId = 1,
                            Title = "Wild",
                            Body = "Body",
                            Version = default(byte[])
                        }
                    });

                var y = await server.CreateClient()
                    .GetAsync<GetNoteByIdQuery.Response>(Get.NoteById(1));

                var response = await server.CreateClient()
                    .PostAsync(Post.Notes, new 
                    {
                        Note = new 
                        {
                            NoteId = 1,
                            Title = "Title WTF",
                            Body = "Body",
                            Version = default(byte[])
                        }
                    });
                
                Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
            }
        }

        [Fact]
        public async Task ShouldFailConcurrentVersion()
        {
            using (var server = CreateServer())
            {
                await server.CreateClient()
                    .PostAsync(Post.Notes, new
                    {
                        Note = new
                        {
                            Title = "Title",
                            Body = "<p>Something Important</p>",
                        }
                    });

                await server.CreateClient()
                    .PostAsync(Post.Notes, new
                    {
                        Note = new
                        {
                            NoteId = 1,
                            Title = "Title",
                            Body = "<p>Something Important</p>",
                        }
                    });

                var task1 = server.CreateClient()
                    .PostAsync(Post.Notes, new
                    {
                        Note = new
                        {
                            NoteId = 1,
                            Title = "Title",
                            Body = "Body",
                            Version = 1
                        }
                    });

                var task2 = server.CreateClient()
                    .PostAsync(Post.Notes, new
                    {
                        Note = new
                        {
                            NoteId = 1,
                            Title = "Title",
                            Body = "Body",
                            Version = 1
                        }
                    });

                var result = await Task.WhenAll(task1, task2);
                
                Assert.Single(result.Where(x => x.StatusCode == System.Net.HttpStatusCode.OK));

                Assert.Single(result.Where(x => x.StatusCode == System.Net.HttpStatusCode.InternalServerError));
            }
        }
    }
}
