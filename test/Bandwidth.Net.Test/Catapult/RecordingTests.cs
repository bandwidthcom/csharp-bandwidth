﻿using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bandwidth.Net.Catapult;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test.Catapult
{
  public class RecordingTest
  {
    [Fact]
    public void TestList()
    {
      var response = new HttpResponseMessage
      {
        Content =
          new JsonContent($"[{Helpers.GetCatapultResourse("Recording")}]")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidListRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetCatapultApi(context).Recording;
      var recordings = api.List();
      ValidateRecording(recordings.First());
    }

    [Fact]
    public async void TestGet()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetCatapultContent("Recording")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetCatapultApi(context).Recording;
      var recording = await api.GetAsync("id");
      ValidateRecording(recording);
    }

    public static bool IsValidListRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/recordings";
    }

    public static bool IsValidGetRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/recordings/id";
    }

    private static void ValidateRecording(Recording item)
    {
      Assert.Equal("recordingId", item.Id);
      Assert.Equal(RecordingState.Complete, item.State);
      Assert.Equal("{callId1}", item.CallId);
      Assert.Equal("{callId1}-1.wav", item.MediaName);
    }
  }
}
