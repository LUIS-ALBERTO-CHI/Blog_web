import { Workbook } from 'exceljs';
import { exportDataGrid } from 'devextreme/excel_exporter';
import HttpService from "@/Fwamework/Core/Services/http-service";

export const onExporting = (e, fileName, excelExportOptions = {}) => {
	const workbook = new Workbook();
	const worksheet = workbook.addWorksheet('Main sheet');
	exportDataGrid({
		component: e.component,
		worksheet: worksheet,
		...excelExportOptions
	}).then(function () {
		workbook.xlsx.writeBuffer()
			.then(function (buffer) {
				HttpService.saveBlobFile(new Blob([buffer]), false, `${fileName}.xlsx`);
			});
	});
};