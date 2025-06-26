import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavbarComponent } from './components/navbar/navbar.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { DialogComponent } from './components/dialog/dialog.component';
import { CreateDeviceComponent } from './components/create-device/create-device.component';
import { TableDataComponent } from './components/table-data/table-data.component';
import { ChartDataComponent } from './components/chart-data/chart-data.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { HttpClientModule } from '@angular/common/http';
import { MatDialogModule } from '@angular/material/dialog';
import { PieChartComponent } from './components/pie-chart/pie-chart.component';



@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    DialogComponent,
    CreateDeviceComponent,
    TableDataComponent,
    ChartDataComponent,
    PieChartComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    HttpClientModule,


    FormsModule,
    MatDialogModule,

    //MatInputModule,
    //MatSelectModule,
    //MatRadioModule,
   
    //MatTableModule,
   
    //MatInputModule,
    //MatDatepickerModule,
    //MatNativeDateModule,
  
      ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
