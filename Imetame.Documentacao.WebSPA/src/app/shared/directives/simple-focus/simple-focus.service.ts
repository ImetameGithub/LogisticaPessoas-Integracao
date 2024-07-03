import { Injectable } from '@angular/core';
import { SimpleFocusDirective } from './simple-focus.directive';

@Injectable({
  providedIn: 'root'
})
export class SimpleFocusService {

    private _registry: { [key: string]: SimpleFocusDirective } = {};

    constructor() { }

    /**
    * Add the sidebar to the registry
    *
    * @param key
    * @param sidebar
    */
    register(key, sidebar): void {
        // Check if the key already being used
        if (this._registry[key]) {
            console.error(`The SimpleFocus with the key '${key}' already exists. Either unregister it first or use a unique key.`);

            return;
        }

        // Add to the registry
        this._registry[key] = sidebar;
    }

    /**
     * Remove the sidebar from the registry
     *
     * @param key
     */
    unregister(key): void {
        // Check if the sidebar exists
        if (!this._registry[key]) {
            console.warn(`The SimpleFocus with the key '${key}' doesn't exist in the registry.`);
        }

        // Unregister the sidebar
        delete this._registry[key];
    }

    /**
     * Return the element with the given key
     *
     * @param key
     * @returns {FuseSidebarComponent}
     */
    getSimpleFocus(key): SimpleFocusDirective {
        // Check if the sidebar exists
        if (!this._registry[key]) {
            console.warn(`The SimpleFocus with the key '${key}' doesn't exist in the registry.`);

            return;
        }

        // Return the sidebar
        return this._registry[key];
    }

    focusOn(key): void {
        this.getSimpleFocus(key).setFocus();
    }
}
