import { Injectable, TemplateRef  } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  toasts: any[] = [];

  // Push new Toasts to array with content and options
  show(textOrTpl: string | TemplateRef<any>, options: any = {}) {
    this.toasts.push({ textOrTpl, ...options });
  }

  // Callback method to remove Toast DOM element from view
  remove(toast: any) {
    this.toasts = this.toasts.filter(t => t !== toast);
  }

  clear() {
		this.toasts.splice(0, this.toasts.length);
	}

	showSuccess(text: string, time: number) {
		this.show(text, { classname: 'bg-success text-white mb-3', delay: time, autohide: true, animation: true});
	}

	showDanger(text: string, time: number) {
		this.show(text, { classname: 'bg-danger text-white mb-3', delay: time, autohide: true, animation: true});
	}
}