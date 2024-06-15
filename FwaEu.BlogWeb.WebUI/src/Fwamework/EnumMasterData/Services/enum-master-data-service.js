import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import HttpService from "@/Fwamework/Core/Services/http-service";
import { CustomDataLoaderService } from "@/Fwamework/MasterData/Services/custom-data-loader-service";
import MasterDataManagerService from "@/Fwamework/MasterData/Services/master-data-manager-service";
import { I18n } from "@/Fwamework/Culture/Services/localization-service";

class EnumDataLoader extends CustomDataLoaderService {
    _enumTypeName = null;
    constructor(enumTypeName) {
        super();
        this._enumTypeName = enumTypeName; 
    } 
    async getMasterDataAsync(_) {//NOTE: masterDataParameters
		const response = await HttpService.get(`EnumValues/Get?enumTypeName=${this._enumTypeName}`);
		const data = response.data;
		data.values = data.values.map(x => { return {'id': x}; })
		return data;//NOTE: le controlleur doit renvoyer RelatedMasterDataGetModelsResult
    }
    async getMasterDataChangeInfoAsync() {
        const response = await HttpService.get(`EnumValues/GetChangeInfos?enumTypeName=${this._enumTypeName}`);

        response.data.maximumUpdatedOn = new Date(response.data.maximumUpdatedOn);//Force to have a Date value instead of string

        return response.data;//note: utiliser RelatedMasterDataChangesInfo
	}
}
class EnumMasterDataService extends MasterDataService {
    _enumTypeName = null;
    constructor(enumTypeName) {
        super(enumTypeName, ['id']);
        this._enumTypeName = enumTypeName;
        this.configuration.customDataLoaderService = new EnumDataLoader(enumTypeName);
    }
    createItem(enumValue) {
        return {
            id: enumValue.id,
            text: I18n.t(enumValue.id),
            toString: function () {
                return this.text;
            }
        }
    }

    async clearCacheAsync() {
        await MasterDataManagerService.clearCacheAsync([this._enumTypeName]);
	}

	async getByIdsAsync(keys)
	{
		return await MasterDataManagerService.getMasterDataByKeysAsync(this.configuration.masterDataKey, keys);
	}
}

export default EnumMasterDataService;