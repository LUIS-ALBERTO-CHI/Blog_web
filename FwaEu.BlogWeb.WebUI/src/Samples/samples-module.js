import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';

export class SamplesModule extends AbstractModule {

	getMenuItemsAsync(menuType) {
		return menuType !== "sideNavigation" ? [] : [{
			text: "Styles",
			icon: "fad fa-palette",
			color: "#0a7dd8",
			items: [{
				text: "Layout",
				path: { name: "LayoutSample" },
				icon: "fal fa-cube"
			},
			{
				text: "Box",
				path: { name: "SampleBox" },
				icon: "fal fa-box"
			},
			{
				text: "Icones",
				path: { name: "SampleIcone" },
				icon: "fal fa-icons"
			}
			]
		},
		{
			text: "Development",
			icon: "fad fa-laptop-code",
			color: "#189882",
			items: [
				{
					text: "Localization",
					path: { name: "LocalizationSample" },
					icon: "fal fa-globe-europe"
				},
				{
					text: "Errors",
					path: { name: "ErrorSample" },
					icon: "fal fa-exclamation-triangle"
				}
			]
		},
		{
			text: "Ergonomics",
			icon: "fad fa-smile-beam",
			color: "#da3f02",
			items: [{
				text: "Breadcrumbs",
				path: { name: "Page1" },
				icon: "fal fa-angle-double-right"
			},
			{
				text: "Loading panel",
				path: { name: "SampleLoadingPanel" },
				icon: "fal fa-redo-alt"
			},
			{
				text: "ActionMenu",
				path: { name: "SampleActionMenu" },
				icon: "fal fa-caret-square-right"
			},
			{
				text: "Dialog",
				path: { name: "SampleDialog" },
				icon: "fal fa-browser"
			},
			{
				text: "Notification",
				path: { name: "SampleNotification" },
				icon: "fal fa-bell"
			},
			{
				text: "User notifications",
				path: { name: "SampleUserNotification" },
				icon: "fal fa-user-music"
			},
			{
				text: "User Tooltip",
				path: { name: "SampleUserToolTip" },
				icon: "fal fa-user-tie"
			},
			{
				text: "User Avatar",
				path: { name: "SampleUserAvatar" },
				icon: "fal fa-user-circle"
			}
			]
		},
		{
			text: "Page container",
			icon: "fad fa-columns",
			color: "#154bbf",
			items: [{
					text: "Form",
					path: { name: "SampleFormPure" },
					icon: "fal fa-file-edit"
				},
				{
					text: "FormBuilder",
					path: { name: "SampleFormBuilder" },
					icon: "fal fa-file-edit"
				},
				{
					text: "List",
					path: { name: "SampleListPure" },
					icon: "fal fa-list-ul"
				},
				{
					text: "Summary",
					path: { name: "SampleSummaryPure" },
					icon: "fal fa-file-alt"
				}
			]
		},
		{
			text: "Data grids",
			icon: "fad fa-table",
			color: "#154bbf",
			items: [{
				text: "Single MasterData DevExtremeDataGrid",
				path: { name: "SampleDataGridDevExtremWithSingleLookupColumns" },
				icon: "fal fa-file-edit"
			},
			{
				text: "Batched MasterData DevExtremeDataGrid",
				path: { name: "SampleDataGridDevExtremWithBatchedLookupColumns" },
				icon: "fal fa-file-edit"
			}]
		}
		];
	}
}