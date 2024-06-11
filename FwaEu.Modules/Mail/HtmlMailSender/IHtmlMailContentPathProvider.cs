using System;
using System.Collections.Generic;

namespace FwaEu.Modules.Mail.HtmlMailSender
{
	public class ContentModel
	{
		public ContentModel(string id, string path)
		{
			Id = id ?? throw new ArgumentNullException(nameof(id));
			Path = path ?? throw new ArgumentNullException(nameof(path));
		}

		/// <summary>
		/// This parameter will be used by Razor when we attach the content as LinkedResources in the BodyBuilder to link the cshtml and the content
		/// </summary>
		public string Id { get; set; }
		public string Path { get; set; }
	}

	public interface IHtmlMailContentPathProvider
	{
		void EnrichParameters(Dictionary<string, object> parameters);
		IEnumerable<ContentModel> GetAllContents();
	}
}
