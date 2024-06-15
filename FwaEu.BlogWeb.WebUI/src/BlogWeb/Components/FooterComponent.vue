<template>
	<footer class="footer">
		{{ version }}
		-
		Copyright © {{ copyrightYears }}, <a href="https://www.fwa.fr" target="_blank">FWA</a>
		<div class="logos">
			<a href="https://www.fwa.fr" target="_blank">
				<img class="logo" alt="" src="../Content/logo-fwa.png" />
			</a>
			<img class="logo" alt="" src="../Content/logo.png" />
		</div>
	</footer>
</template>

<script>
	import ApplicationInfoService from '@/Fwamework/Core/Services/application-info-service';
    import { loadMessagesAsync } from '@/Fwamework/Culture/Services/single-file-component-localization';
    import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';

    export default {
        data() {
			return {
				appInfo: ApplicationInfoService.get()
			}
		},
        created: showLoadingPanel(async function () {
            await loadMessagesAsync(this, import.meta.glob('./Content/footer-messages.*.json'));
        }),
        computed: {
			version() {
				return this.$t("versionMask", [this.appInfo?.version]);
			},
			copyrightYears() {
				return this.appInfo?.copyrightYears;
			}
		}
    }
</script>

<style type="text/css" src="../Content/footer.css"></style>
