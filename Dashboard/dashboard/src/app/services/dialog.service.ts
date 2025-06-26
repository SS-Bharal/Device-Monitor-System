import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../components/dialog/dialog.component';

@Injectable({
  providedIn: 'root'
})
export class DialogService {
  constructor(private dialog: MatDialog) { }

  openDialog(message: string): void {

    this.dialog.open(DialogComponent, {
      width: '400px',
      data: { message: message }
    });


  }
}
