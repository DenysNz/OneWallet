import { Component, OnInit, TemplateRef } from '@angular/core';
import { ToastService } from '../core/services/toasts.service';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss'],
  host: {class: "ngb-toasts position-fixed top-0 end-0 p-3", style: "z-index: 1200;" }
})
export class ToastComponent implements OnInit {

  constructor(public toastService: ToastService) {}

  ngOnInit(): void {
  }

  isTemplate(toast: any) { return toast.textOrTpl instanceof TemplateRef; }
}
