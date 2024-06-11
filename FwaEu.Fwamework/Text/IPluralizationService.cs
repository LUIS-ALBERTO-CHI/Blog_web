using Pluralize.NET.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Text
{
	public interface IPluralizationService
	{
		string Pluralize(string word);
		string Singularize(string word);
	}

	public class PluralizeNetCorePluralizationService : IPluralizationService
	{
		protected Pluralizer Pluralizer { get; } = new Pluralizer();

		public virtual string Pluralize(string word)
		{
			return this.Pluralizer.Pluralize(word);
		}

		public virtual string Singularize(string word)
		{
			return this.Pluralizer.Singularize(word);
		}
	}
}
