import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { LowerCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { BucketListService } from '../../core/services/bucket-list.service';
import { BucketItemResponse, CreateBucketItemRequest } from '../../shared/models/bucket-item.model';

@Component({
  selector: 'app-dashboard',
  imports: [LowerCasePipe, FormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  private bucketService = inject(BucketListService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  items: BucketItemResponse[] = [];
  randomItem: BucketItemResponse | null = null;
  username = 'You';
  errorToast = '';

  newItem: CreateBucketItemRequest = {
    title: '',
    description: null,
    priority: 'MEDIUM',
    categoryId: null,
  };

  ngOnInit(): void {
    this.loadAllItems();
  }

  loadAllItems(): void {
    this.bucketService.getItems().subscribe({
      next: (data) => {
        this.items = data;
        this.cdr.markForCheck();
      },
      error: () => {
        // Token expired or unauthorized — redirect to login
        this.router.navigate(['/login']);
      },
    });
  }

  addItem(): void {
    if (!this.newItem.title.trim()) return;

    this.bucketService.createItem(this.newItem).subscribe({
      next: (created) => {
        this.items.unshift(created);
        this.newItem = { title: '', description: null, priority: 'MEDIUM', categoryId: null };
        this.errorToast = '';
        this.cdr.markForCheck();
      },
      error: () => {
        this.errorToast = 'Could not save goal. Please try again.';
        this.cdr.markForCheck();
      },
    });
  }

  toggleComplete(item: BucketItemResponse): void {
    const next = item.isCompleted === 1 ? 0 : 1;

    this.bucketService.updateItem(item.itemId, {
      title: item.title,
      description: item.description,
      priority: item.priority,
      categoryId: item.categoryId,
      isCompleted: next,
    }).subscribe({
      next: () => {
        item.isCompleted = next;
        this.cdr.markForCheck();
      },
      error: () => {
        this.errorToast = 'Could not update goal status. Please try again.';
        this.cdr.markForCheck();
        this.loadAllItems();
      },
    });
  }

  deleteItem(id: number): void {
    this.bucketService.deleteItem(id).subscribe({
      next: () => {
        this.items = this.items.filter((i) => i.itemId !== id);
        this.errorToast = '';
        this.cdr.markForCheck();
      },
      error: () => {
        this.errorToast = 'Could not delete goal. Please try again.';
        this.cdr.markForCheck();
      },
    });
  }

  clickRandom(): void {
    this.bucketService.pickRandom().subscribe({
      next: (data) => {
        this.randomItem = data;
        this.errorToast = '';
        this.cdr.markForCheck();
      },
      error: () => {
        this.errorToast = 'No incomplete goals left to pick from.';
        this.cdr.markForCheck();
      },
    });
  }

  logout(): void {
    localStorage.removeItem('jwtToken');
    this.router.navigate(['/login']);
  }

  get completedCount(): number {
    return this.items.filter((i) => i.isCompleted === 1).length;
  }
}
