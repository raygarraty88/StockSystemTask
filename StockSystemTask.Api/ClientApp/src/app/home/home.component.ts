import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ChartConfiguration } from 'chart.js';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  public stockPrices: StockPrice[] = [];
  public showLoading = false;
  public dataLoaded = false;
  public baseUrl = '';
  public symbol = '';
  public lineChartData: ChartConfiguration['data'] = { datasets: [] };
  public lineChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: true,
  }

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  onSubmit() {
    this.showLoading = true;
    this.dataLoaded = false;
    this.http.get<StockPrice[]>(`${this.baseUrl}AlphaVantagePrices?symbol=${this.symbol}`).subscribe(result => {
      this.showLoading = false;
      this.dataLoaded = true;
      this.stockPrices = result.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
      this.lineChartData = {
        datasets: [
          {
            data: this.stockPrices.filter(price => price.symbol == this.symbol).map(price => price.performance),
            label: this.symbol,
            backgroundColor: 'rgba(148,159,177,0.2)',
            borderColor: 'rgba(148,159,177,1)',
            pointBackgroundColor: 'rgba(148,159,177,1)',
            pointBorderColor: '#fff',
            pointHoverBackgroundColor: '#fff',
            pointHoverBorderColor: 'rgba(148,159,177,0.8)',
            fill: 'origin',
          },
          {
            data: this.stockPrices.filter(price => price.symbol == 'SPY').map(price => price.performance),
            label: 'SPY',
            backgroundColor: 'rgba(77,83,96,0.2)',
            borderColor: 'rgba(77,83,96,1)',
            pointBackgroundColor: 'rgba(77,83,96,1)',
            pointBorderColor: '#fff',
            pointHoverBackgroundColor: '#fff',
            pointHoverBorderColor: 'rgba(77,83,96,1)',
            fill: 'origin',
          }
        ],
        labels: this.stockPrices.filter(price => price.symbol == 'SPY').map(price => new Date(price.date).toISOString().split('T')[0])
      };
    }, error => {
      console.error(error);
      this.showLoading = false;
    });
  }
}

interface StockPrice {
  date: string;
  symbol: string;
  performance: number;
}
