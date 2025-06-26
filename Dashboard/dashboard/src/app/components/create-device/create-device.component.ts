import { Component, OnInit } from '@angular/core';
//import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import { DialogService } from '../../services/dialog.service';
import { DeviceData } from '../../Models/DeviceData';
import { Router } from '@angular/router';






@Component({
  selector: 'app-create-device',
  templateUrl: './create-device.component.html',
  styleUrls: ['./create-device.component.css']
})
export class CreateDeviceComponent {

  //deviceForm: FormGroup;
  alarmTypes: any;
  alarmList: string[] = [];
  tempData: any;
  createData: any;

  deviceStates: { [deviceId: string]: string } = {};
  selectedDevices: string[] = []; // To store selected devices
 


  getApiAlarm = environment.GetApiUrlForAlarm;
  insertApi = environment.postApiUrlForDevice;

  //constructor(private fb: FormBuilder) {

  //  this.deviceForm = this.fb.group({
  //    deviceCode: ['', Validators.required],
  //    deviceName: ['', Validators.required],
  //    alarmInterval: ['', [Validators.required, Validators.min(1)]],
  //    alarmTypes: this.fb.array([], Validators.required)
  //  });

  //}

  ngOnInit() {
    this.getAlarmList();
  }

  deviceForm: FormGroup;
  isCheckboxSelected: boolean = false;

  constructor(private fb: FormBuilder, private http: HttpClient, private dialogService: DialogService, private router: Router) {

    this.deviceForm = this.fb.group({
      deviceCode: ['', [Validators.required]],
      deviceName: ['', [Validators.required, Validators.minLength(3)]],
      alarmInterval: [null, [Validators.required, Validators.min(0)]],
      
    });


  }

  onSelectDevice(deviceId: string) {
    if (this.selectedDevices.includes(deviceId)) {
      // Deselect the device if it's already selected
      this.selectedDevices = this.selectedDevices.filter(id => id !== deviceId);
    } else {
      // Select the device if it's not already selected
      this.selectedDevices.push(deviceId);
    }
    this.isCheckboxSelected = this.selectedDevices.length > 0;
  }

  getDeviceStateKeys() {
    return Object.keys(this.deviceStates);
  }


  getAlarmList(): void{

    this.http.get(this.getApiAlarm)
      .subscribe((data) => {
        //debugger;
        this.tempData = data;
        this.alarmTypes = this.tempData.result;

        this.alarmTypes.forEach((alarm: { alarmId: string, alarmName: string }) => {
          this.deviceStates[alarm.alarmId] = alarm.alarmName;
        });

        console.log('GET Request Response of Devices List:', this.deviceStates);


      }, (error) => {
        console.error('GET Request Error:', error);
      });

  }


  onSubmit() : void {
    if (this.deviceForm.valid) {
      // Handle form submission here, e.g., send data to a server.
      const formData = this.deviceForm.value;
      debugger;

      for (let i = 0; i<this.selectedDevices.length; i++) {
        this.alarmList.push(this.selectedDevices[i]);
      }

      const DData = new DeviceData();
      DData.DeviceCode = formData.deviceCode;
      DData.DeviceName = formData.deviceName;
      DData.AlarmInterval = formData.alarmInterval;
      DData.AlarmId = this.alarmList;

      //console.log("Alarm List {0}", DData.AlarmId);
     

      debugger;
      this.http.post(this.insertApi, DData).subscribe(
        (response) => {
          // Handle success response here
          console.log('Data posted successfully', response);
          var customMessage="Device Inserted success"
          this.dialogService.openDialog(customMessage);

          this.router.navigate(['']);

        },
        (error) => {
          // Handle error here
          console.error('Error posting data', error.error);
          var customMessage = "Failed to Insert Device , Device Already Exist "
          this.dialogService.openDialog(customMessage);
          //this.openPopup(error.error);

        }
      );



     
    }
  }




}
