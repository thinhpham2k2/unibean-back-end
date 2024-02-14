using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Net.Http.Headers;
using System.Text;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.WebHooks;
using Unibean.Service.Services.Interfaces;
using static Unibean.Service.Models.WebHooks.DiscordWebhookModel;
using File = Unibean.Service.Models.WebHooks.DiscordWebhookModel.File;

namespace Unibean.Service.Services;

public class DiscordService : IDiscordService
{
    public async void CreateWebHooks(DiscordWebhookModel webhookModel)
    {
        try
        {
            if (string.IsNullOrEmpty(webhookModel.Content) && webhookModel.Embeds.Count == 0)
            {
                throw new InvalidParameterException("Đặt nội dung hoặc thêm ít nhất một EmbedObject");
            }

            JObject json = new()
            {
                ["content"] = webhookModel.Content,
                ["username"] = webhookModel.Username,
                ["avatar_url"] = webhookModel.AvatarUrl,
                ["tts"] = webhookModel.Tts
            };

            if (webhookModel.Embeds.Count > 0)
            {
                List<JObject> embedObjects = new();

                foreach (EmbedObject embed in webhookModel.Embeds)
                {
                    JObject jsonEmbed = new()
                    {
                        ["title"] = embed.Title,
                        ["description"] = embed.Description,
                        ["url"] = embed.Url
                    };

                    Color color = embed.Color;
                    int rgb = (color.R << 16) + (color.G << 8) + color.B;

                    jsonEmbed["color"] = rgb;

                    Footer footer = embed.Footer;
                    Image image = embed.Image;
                    Thumbnail thumbnail = embed.Thumbnail;
                    Author author = embed.Author;
                    List<Field> fields = embed.Fields;

                    if (footer != null)
                    {
                        JObject jsonFooter = new()
                        {
                            ["text"] = footer.Text,
                            ["icon_url"] = footer.IconUrl
                        };
                        jsonEmbed["footer"] = jsonFooter;
                    }

                    if (image != null)
                    {
                        JObject jsonImage = new()
                        {
                            ["url"] = image.Url
                        };
                        jsonEmbed["image"] = jsonImage;
                    }

                    if (thumbnail != null)
                    {
                        JObject jsonThumbnail = new()
                        {
                            ["url"] = thumbnail.Url
                        };
                        jsonEmbed["thumbnail"] = jsonThumbnail;
                    }

                    if (author != null)
                    {
                        JObject jsonAuthor = new()
                        {
                            ["name"] = author.Name,
                            ["url"] = author.Url,
                            ["icon_url"] = author.IconUrl
                        };
                        jsonEmbed["author"] = jsonAuthor;
                    }

                    List<JObject> jsonFields = new();
                    foreach (Field field in fields)
                    {
                        JObject jsonField = new()
                        {
                            ["name"] = field.Name,
                            ["value"] = field.Value,
                            ["inline"] = field.Inline
                        };

                        jsonFields.Add(jsonField);
                    }

                    jsonEmbed["fields"] = JArray.FromObject(jsonFields);
                    embedObjects.Add(jsonEmbed);
                }

                json["embeds"] = JArray.FromObject(embedObjects);
            }

            // Tạo MultipartFormDataContent
            MultipartFormDataContent formDataContent = new();

            foreach (File file in webhookModel.Files)
            {
                byte[] fileBytes = await new HttpClient().GetAsync(file.Attachment).Result.Content.ReadAsByteArrayAsync();

                // Tạo ByteArrayContent từ mảng byte của file
                ByteArrayContent fileContent = new(fileBytes);

                // Đặt tên file
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = file.Name
                };

                // Thêm fileContent vào formDataContent
                formDataContent.Add(fileContent);
            }


            string url = "https://discord.com/api/webhooks/1200391596848447558/baMHasDk0eVMr2wjzXQxc6VJ9Fku3umtEcMWQjTcqW2kf7wm_G7sl9SMS-JaQdgHdGCB";

            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("C#-DiscordWebhook-BY-Gelox_");

            StringContent content = new(json.ToString(), Encoding.UTF8, "application/json");

            // Đặt tên và thêm jsonContent vào formDataContent
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "payload_json"
            };

            formDataContent.Add(content);

            HttpResponseMessage response = await httpClient.PostAsync(url, formDataContent);

            response.EnsureSuccessStatusCode();
            httpClient.Dispose();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
