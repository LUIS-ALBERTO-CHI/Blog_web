import { I18n } from '@/Fwamework/Culture/Services/localization-service';

function createInfoEntry(summary, displayableEntries)
{
	return { type: "Info", content: summary, details: displayableEntries ? [displayableEntries] : null };
}

function formatEntries(entries)
{
	const maxDisplayedEntries = 50;

	var displayable = entries
		.slice(0, maxDisplayedEntries)
		.map(e => e.content)
		.join(', ');

	if (entries.length > maxDisplayedEntries)
	{
		const notDisplayedCount = entries.length - maxDisplayedEntries;
		displayable += "... " + I18n.t("andOther", notDisplayedCount);
	}

	return displayable;
}

function Store()
{
	this.added = [];
	this.updated = [];
	this.others = [];

	this.createEntries = function ()
	{
		var result = Array.from(this.others);

		if (this.added.length === 0 && this.updated.length === 0)
		{
			result.push(createInfoEntry(I18n.t("nothingCreatedOrUpdated")));
		}
		else
		{
			if (this.added.length)
			{
				result.push(createInfoEntry(
					I18n.t("added", this.added.length),
					formatEntries(this.added)));
			}

			if (this.updated.length)
			{
				result.push(createInfoEntry(
					I18n.t("updated", this.updated.length),
					formatEntries(this.updated)));
			}
		}

		return result;
	};
}

export default
	{
		async customizeAsync(context)
		{
			if (context.processName == "ModelImporter")
			{
				var store = new Store();

				context.entries.forEach(function (entry)
				{
					switch (entry.type)
					{
						case "ModelUpdated":
							store.updated.push(entry);
							break

						case "ModelCreated":
							store.added.push(entry);
							break;

						default:
							store.others.push(entry);
					}
				});

				context.name = I18n.t("importOf", [context.extendedProperties.modelName]);
				context.entries = store.createEntries();
			}

			return context;
		}
	}