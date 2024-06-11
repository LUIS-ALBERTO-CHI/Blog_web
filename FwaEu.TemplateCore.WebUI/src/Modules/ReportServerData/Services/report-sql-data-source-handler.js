import ReportServerDataSourceHandlerBase from "@/Modules/ReportServerData/Services/report-server-data-source-handler-base";
import { defineAsyncComponent } from "vue";

class ReportSqlDataSourceHandler extends ReportServerDataSourceHandlerBase {
	type = "Sql";
	displayOrder = 10;
	icon = "fas fa-database";
	usePreFilters = true;
	useCustomParameters = true;
	createComponent = () => defineAsyncComponent(() => import("@/Modules/ReportServerData/Components/SqlQueryComponent.vue"));
	isRequired = true;
	getDescription() {
		return {
			// TODO: ici il faut songer à ajouter une description expliquant la gestion des laod parameters configurés
			// https://fwaeu.visualstudio.com/TemplateCore/_workitems/edit/7354
			label: 'Sql'
		};
	}	
}

export default ReportSqlDataSourceHandler;

