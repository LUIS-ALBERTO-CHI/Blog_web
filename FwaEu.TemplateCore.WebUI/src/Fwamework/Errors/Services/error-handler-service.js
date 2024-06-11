import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
import AsyncEventEmitter from "@/Fwamework/Core/Services/event-emitter-class";
const  context  = import.meta.glob('/**/default-error-handler.js', { import: 'default', eager:true });

const errorEventEmitter = new AsyncEventEmitter();

/**
 * @typedef {{isHandled: Boolean, error: any}} ErrorParameter
 * @typedef {{onErrorAsync: (error: ErrorParameter) => Promise<void>}} ErrorHandler
 * @type {Array<ErrorHandler>}
 */
const defaultErrorHandlers = Object.values(context);
const defaultErrorHandler = defaultErrorHandlers.find(ns => ns.key === Configuration.fwamework.errors.handlerKey);

/**
 * @type {Array<ErrorHandler>}
 * */
const customErrorHandlers = [];

export default {
	configure(vueApp) {

		const $this = this;
		vueApp.config.errorHandler = (error, vue) => {
			error.vue = vue;
			$this.dispatchError(error);
		}
		window.addEventListener('unhandledrejection', this.dispatchError);
	},

	/** @param {() => Promise} listener */
	onUnhandledError(listener) {
		return errorEventEmitter.addListener(listener);
	},

	/**
	 * @param {ErrorHandler} errorHandler
	 */
	registerErrorHandler(errorHandler) {
		customErrorHandlers.push(errorHandler);
	},

	/**
	 * @param {{isHandled: Boolean}} error
	 */
	dispatchError(error) {

		//Handle special case of some libraries that throws empty errors
		if (!error) {
			error = new Error("Unknown error");
		}

		//Unwrap error object for promises
		if (error.constructor?.name === "PromiseRejectionEvent") {
			error = error.reason;
		}

		//Ignore if error was already handled
		if (error.isHandled)
			return;
		else error.isHandled = false;

		errorEventEmitter.emitAsync(error).catch((e) => {
			//Prevent infinite loops
			console.error(e);
		});

		//Because the error events are synchronous and listeners can be async, we need to chain the error handler promises
		customErrorHandlers
			.concat([defaultErrorHandler])//Add default handler at the end
			.reduce((chainedPromise, handler) => {
				return chainedPromise.then(async () => {

					//Stop propagation if error was handled
					if (!error.isHandled) {
						try {

							await handler.onErrorAsync(error);
						} catch (e) {
							//Prevent infinite loops
							console.error(e);
						}
					}
				});
			}, Promise.resolve());
	}
}
