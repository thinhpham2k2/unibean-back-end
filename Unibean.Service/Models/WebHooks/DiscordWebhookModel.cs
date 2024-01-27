using System.Drawing;
using System.Text;

namespace Unibean.Service.Models.WebHooks;

public class DiscordWebhookModel
{
    public string Content { get; set; }

    public string Username = "Unibean Webhooks Bot";

    public readonly string AvatarUrl = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/accounts%2Flogo_justB.png?alt=media&token=b56e6c5a-af58-4d97-a212-14598e0ddcf8";

    public bool Tts { get; set; }

    public List<EmbedObject> Embeds = new();

    public List<File> Files = new();

    public class EmbedObject
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public Color Color = Color.FromArgb(34, 139, 34);

        public Footer Footer { get; set; }

        public Thumbnail Thumbnail { get; set; }

        public Image Image { get; set; }

        public Author Author { get; set; }

        public List<Field> Fields = new();
    }

    public record Footer()
    {
        public string Text { get; set; }

        public string IconUrl { get; set; }
    }

    public record Thumbnail()
    {
        public string Url { get; set; }
    }

    public record Image()
    {
        public string Url { get; set; }
    }

    public record Author()
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string IconUrl { get; set; }

    }

    public record Field()
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Inline { get; set; }
    }

    public record File()
    {
        public string Attachment { get; set; }

        public string Name { get; set; }
    }

    public class JSONObject
    {
        private readonly Dictionary<string, object> map = new();

        public void Put(string key, object value)
        {
            if (value != null)
            {
                map[key] = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new();
            builder.Append('{');

            int i = 0;
            foreach (KeyValuePair<string, object> entry in map)
            {
                object val = entry.Value;
                builder.Append(Quote(entry.Key)).Append(':');

                if (val is string)
                {
                    builder.Append(Quote(val.ToString()));
                }
                else if (val is int @int)
                {
                    builder.Append(@int);
                }
                else if (val is bool boolean)
                {
                    builder.Append(boolean ? "true" : "false");
                }
                else if (val is JSONObject)
                {
                    builder.Append(val.ToString());
                }
                else if (val.GetType().IsArray)
                {
                    builder.Append('[');
                    Array array = (Array)val;
                    int len = array.Length;
                    for (int j = 0; j < len; j++)
                    {
                        builder.Append(array.GetValue(j).ToString()).Append(j != len - 1 ? "," : "");
                    }
                    builder.Append(']');
                }

                builder.Append(++i == map.Count ? "}" : ",");
            }

            return builder.ToString();
        }

        private static string Quote(string str)
        {
            return "\"" + str + "\"";
        }
    }
}
