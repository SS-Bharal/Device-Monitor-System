import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import Chart from 'chart.js/auto';
import { environment } from '../../../environments/environment.development';
import { DialogService } from '../../services/dialog.service';



@Component({
  selector: 'app-chart-data',
  templateUrl: './chart-data.component.html',
  styleUrls: ['./chart-data.component.css']
})
export class ChartDataComponent {

  myForm: FormGroup;
  tempData: any;
  rawData: any;

  pieChartData: number[] = [];
  pieChartLabels: string[] = [];

  public chart: any;


  getApiChart = environment.GetApiUrlForChart;

  ngOnInit() {
    //this.createChart();
  }

  constructor(private fb: FormBuilder, private http: HttpClient, private dialogService: DialogService) {
    this.myForm = this.fb.group({
      deviceCode: ['', Validators.required],
      date: [null, Validators.required]
    });
  }


  onSubmit() {
    if (this.myForm.valid) {
      const formData = this.myForm.value;
      debugger;
      var deviceName = formData;

      this.http.get(`${this.getApiChart}/${formData.deviceCode}/${formData.date}`)
        .subscribe((data) => {

          debugger;
          this.tempData = data;
          this.rawData = this.tempData.result;
          console.log('GET Request Response of Devices List:', this.rawData);

          for (const key in this.rawData) {
            if (this.rawData.hasOwnProperty(key)) {
              console.log(this.rawData, key);
              this.pieChartLabels.push(key);
              this.pieChartData.push(this.rawData[key]);
            }
          }
          if (this.pieChartLabels.length == 0 && this.pieChartData.length == 0) {
            this.dialogService.openDialog("No Data Exist For this Date");
          }
          this.createChart();
          
          debugger;
          //var customMessage = "Device Inserted"
          //this.dialogService.openDialog(customMessage);

        }, (error) => {
          console.error('GET Request Error:', error);
        });

    }
  }


  createChart() {
    console.log(this.rawData);
    debugger;
    this.chart = new Chart("SahilChart", {
      type: 'pie', //this denotes tha type of chart

      data: {// values on X-Axis
        labels: this.pieChartLabels,
        //labels: ['Red', 'Pink', 'Green', 'Yellow', 'Orange', 'Blue',],
        datasets: [{
          //label: 'My First Dataset',
          data: this.pieChartData,
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

    console.log(this.chart);

  }




}
