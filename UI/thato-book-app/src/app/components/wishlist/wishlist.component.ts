import { Component, OnInit, Injectable } from '@angular/core';
import { Book } from 'src/app/models/book';
import { SubscriptionService } from 'src/app/services/subscription.service';
import { Observable, Subject } from 'rxjs';
import { WishlistService } from 'src/app/services/wishlist.service';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { takeUntil } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-wishlist',
  templateUrl: './wishlist.component.html',
  styleUrls: ['./wishlist.component.scss']
})
@Injectable({
  providedIn: 'root',
})
export class WishlistComponent implements OnInit {
  bookId:any;
  wishlistItems$: Observable<Book[]>;
  isLoading: boolean;
  userId: any;
  private unsubscribe$ = new Subject<void>();


  constructor(
    private subscriptionService: SubscriptionService,
    private wishlistService: WishlistService,
    private route: ActivatedRoute,
    private snackBarService: SnackbarService ) {
    this.userId = localStorage.getItem('userId');
    this.bookId = this.route.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
    this.isLoading = true;
    this.getWishlistItems();

  }

  getWishlistItems() {
    this.wishlistItems$ = this.subscriptionService.wishlistItem$;
    this.isLoading = false;
  }

  clearWishlist() {
    this.wishlistService.clearWishlist(this.userId)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        result => {
          this.subscriptionService.wishlistItemcount$.next(result);
          this.snackBarService.showSnackBar('Wishlist cleared!!!');

        }, error => {
          console.log('Error ocurred while deleting wishlist item : ', error);
        });
  }
}
