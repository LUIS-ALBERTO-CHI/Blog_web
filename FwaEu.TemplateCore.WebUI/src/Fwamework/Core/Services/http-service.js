import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
import axios from 'axios';

/**
 * 
 * @typedef { import("axios").AxiosInstance & { resolveUrl: (url: String) => String, setBaseUrl: (url: String) => void, getFileName: (headers: Object) => String, saveFile: (response: import("axios").AxiosResponse<any>, isOpendFile: Boolean = false, fileName: String = null) => void, saveBlobFile: (blobFile: Blob, isOpendFile: Boolean = false, fileName: String = null) => void } } HttpService
 * @type HttpService
 * */
const axiosInstance = axios.create({
	baseURL: Configuration.fwamework.core.apiEndpoint,
	withCredentials: true
});

axiosInstance.resolveUrl = (url) => {
	return `${axiosInstance.defaults.baseURL}${url}`;
};

axiosInstance.setBaseUrl = (baseUrl) => {
	axiosInstance.defaults.baseURL = baseUrl;
};

axiosInstance.getFileName = (headers) => {
	var filename = "";
	var disposition = headers['content-disposition'];
	if (disposition) {
		var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
		var matches = filenameRegex.exec(disposition);
		if (matches !== null && matches[1]) {
			filename = matches[1].replace(/['"]/g, '');
		}
	}
	return filename;
};

axiosInstance.saveFile = (response, isOpendFile = false, fileName = null) => {
	if (response.data?.constructor?.name !== "Blob")
		throw new Error("The download file function works only with the files having Blob type.");

	const blobFile = response.data;

	if (fileName === null)
		fileName = decodeURIComponent(axiosInstance.getFileName(response.headers));

	axiosInstance.saveBlobFile(blobFile, isOpendFile, fileName);
}

axiosInstance.saveBlobFile = (blobFile, isOpendFile = false, fileName = null) => {
	if (window.navigator && window.navigator.msSaveBlob) {
		window.navigator.msSaveBlob(blobFile, fileName);

	} else {
		const url = window.URL.createObjectURL(blobFile);
		const link = document.createElement('a');
		link.href = url;

		if (isOpendFile === false)
			link.setAttribute('download', fileName);
		document.body.appendChild(link);
		link.click();
		document.body.removeChild(link);
		window.URL.revokeObjectURL(url);
	}
};

export default axiosInstance;