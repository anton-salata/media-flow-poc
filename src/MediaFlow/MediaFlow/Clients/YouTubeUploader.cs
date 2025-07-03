using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MediaFlow.Clients
{
	

	public class YouTubeUploader
	{
		public async Task UploadVideoAsync(string videoFilePath, string title, string description, string[] tags)
		{
			UserCredential credential;

			using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
			{
				credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.FromStream(stream).Secrets,
					new[] { YouTubeService.Scope.YoutubeUpload },
					"user",
					CancellationToken.None,
					new FileDataStore("YouTubeUploader")
				);
			}

			var youtubeService = new YouTubeService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = "YouTubeUploader"
			});

			var video = new Video
			{
				Snippet = new VideoSnippet
				{
					Title = title,
					Description = description,
					Tags = tags,
					CategoryId = "22" // See: https://developers.google.com/youtube/v3/docs/videoCategories/list
				},
				Status = new VideoStatus
				{
					PrivacyStatus = "public" // "private", "unlisted", or "public"
				}
			};

			using var fileStream = new FileStream(videoFilePath, FileMode.Open);
			var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");

			videosInsertRequest.ProgressChanged += progress =>
			{
				Console.WriteLine(progress.Status + " " + progress.BytesSent);
			};

			videosInsertRequest.ResponseReceived += uploadedVideo =>
			{
				Console.WriteLine("Video ID: " + uploadedVideo.Id);
			};

			await videosInsertRequest.UploadAsync();
		}
	}

}
