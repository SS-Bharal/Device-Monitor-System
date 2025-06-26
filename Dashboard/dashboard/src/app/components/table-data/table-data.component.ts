import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../../environments/environment.development';
import { DialogService } from '../../services/dialog.service';

@Component({
  selector: 'app-table-data',
  templateUrl: './table-data.component.html',
  styleUrls: ['./table-data.component.css']
})
export class TableDataComponent {

  myForm: FormGroup;
  tempData: any;
  rawData: any;


  getApiTable = environment.GetApiUrlForTable;


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

      this.http.get(`${this.getApiTable}/${formData.deviceCode}/${formData.date}`)
        .subscribe((data) => {

          debugger;
          this.tempData = data;
          this.rawData = this.tempData.result;
          console.log('GET Request Response of Devices List:', this.rawData);
          debugger;

          if (this.rawData.length == 0) {
            this.dialogService.openDialog("No Data Exist For this Date");
          }
          //var customMessage = "Device Inserted"
          //this.dialogService.openDialog(customMessage);

        }, (error) => {
          console.error('GET Request Error:', error);
        });

    }
  }

}
