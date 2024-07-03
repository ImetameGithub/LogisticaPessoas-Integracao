import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'mat-star-rating',
  templateUrl: './star-rating.component.html',
  styleUrls: ['./star-rating.component.scss'],
  encapsulation: ViewEncapsulation.Emulated
})
export class StarRatingComponent implements OnInit {

  @Input('rating')  rating: number;
  @Input('starCount')  starCount: number;
  @Input('cssClass')  cssClass: string;
  @Input('editable')  editable = false;
  @Output()  ratingUpdated = new EventEmitter();

  //private snackBarDuration: number = 2000;
   ratingArr = [];

  constructor(private snackBar: MatSnackBar) {
  }


  ngOnInit() {
    for (let index = 0; index < this.starCount; index++) {
      this.ratingArr.push(index);
    }
  }
  onClick(rating: number) {
    //console.log(rating)
    //this.snackBar.open('You rated ' + rating + ' / ' + this.starCount, '', {
    //  duration: this.snackBarDuration
    //});
    if (this.editable)
      this.ratingUpdated.emit(rating);
    return false;
  }
  
  showIcon(index: number) {
    const v = index + 1;
    const inteiro = Math.floor(this.rating);
    if (inteiro >= index + 1) {
      return 'star';
    } else if (inteiro < this.rating && Math.ceil(this.rating) >= index + 1) {
      return 'star_half';
    } else {
      return 'star_border';
    }    
  }

}

