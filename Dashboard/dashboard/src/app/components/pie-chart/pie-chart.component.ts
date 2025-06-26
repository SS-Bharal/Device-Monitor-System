import { Component, Input } from '@angular/core';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent {

  @Input() rawData: any;
  pieChartData: number[] = [];
  pieChartLabels: string[] = [];

  public chart: any;
  public AlarmList: any;
  public AlarmCountList: any;


  updateDataSetData(): any {

    for (let i = 0; i < this.rawData.length; i++) {
      this.AlarmList.push(this.rawData[i].AlarmName);
    }

    return this.AlarmList;

  }


  updateDataLables(): any {

    for (let i = 0; i < this.rawData.length; i++) {
      this.AlarmCountList.push(this.rawData[i].AlarmCount);
    }

    return this.AlarmCountList;

  }




  ngOnInit(): void {
    

  }



  ngOnChanges(): void {

    this.updateDataSetData();
    this.updateDataLables();
    this.createChart();
    console.log(this.rawData);
    if (this.rawData) {
      //this.pieChartData = this.rawData.map((item: { label: string, value: number }) => item.value);
      //this.pieChartLabels = this.rawData.map((item: { label: string, value: number }) => item.label);
      this.pieChartData = this.rawData.keys();
      this.pieChartLabels = this.rawData.values();
      console.log("data list", this.pieChartData);
      console.log("data list", this.pieChartLabels);
    }
    debugger;

  }



  createChart() {
    console.log(this.rawData);
    debugger;
    this.chart = new Chart("MyChart", {
      type: 'pie', //this denotes tha type of chart

      data: {// values on X-Axis
        labels: this.updateDataSetData(),
        //labels: ['Red', 'Pink', 'Green', 'Yellow', 'Orange', 'Blue',],
        datasets: [{
          label: 'My First Dataset',
          data: this.updateDataLables(),
          //data: [300, 240, 100, 432, 253, 34],
          backgroundColor:
            [
            'red',
            'pink',
            'green',
            'yellow',
            'orange',
            'blue',
          ],
          hoverOffset: 4
        }],
      },
      options: {
        aspectRatio: 2.5
      }

    });
  }

}
