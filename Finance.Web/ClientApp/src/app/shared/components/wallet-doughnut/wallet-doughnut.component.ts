import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Chart } from 'chart.js/auto';
import { AccountModel } from 'src/app/core/models/account.model';
import { WalletAccountModel } from 'src/app/core/models/wallet-account.model';

@Component({
  selector: 'app-wallet-doughnut',
  templateUrl: './wallet-doughnut.component.html',
  styleUrls: ['./wallet-doughnut.component.scss'],
})
export class WalletDoughnutComponent implements OnInit, AfterViewInit {
  @Input() accounts: WalletAccountModel[] | undefined;
  @Input() currency: string | undefined;
  @Input() accountsSum: number = 0;
  @Input() chartId: string = 'my_chart';

  chart: any;
  totalBalance: number = 0;
  accountsNames: string[] = [];
  accountsBalances: number[] = [];
  colors: string[] = [];

  constructor(private cdRef: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.fillDoughnutFields();
    this.fillColorsArray(this.accountsSum);
  }

  ngAfterViewInit(): void {
    this.createChart();
    this.cdRef.detectChanges();
  }

  createChart(): void {
    this.chart = new Chart(this.chartId, {
      type: 'doughnut',
      data: {
        labels: this.accountsNames,
        datasets: [
          {
            label: 'Account',
            data: this.accountsBalances,
            backgroundColor: this.colors,
            hoverOffset: 4,
          },
        ],
      },
      options: {
        plugins: {
          legend: {
            display: false,
          },
        },
      },
    });
  }

  fillDoughnutFields(): void {
    this.accounts?.forEach((account) => {
      this.accountsBalances.push(account.amount);
      this.accountsNames.push(account.accountName);
      this.totalBalance += account.amount;
    });
  }

  fillColorsArray(accountsSum: number): void {
    let colorId = 259;
    for (let i = 0; i < accountsSum; i++) {
      colorId += 5;
      this.colors.push(`hsl(${colorId}, 60%, 56%)`);
    }
  }
}
