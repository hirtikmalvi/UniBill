import { NgStyle } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-spinner',
  imports: [NgStyle],
  templateUrl: './spinner.component.html',
  styleUrl: './spinner.component.css',
})
export class SpinnerComponent {
  @Input() size: string = '2rem'; // default
}
