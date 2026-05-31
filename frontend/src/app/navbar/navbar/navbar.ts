import { Component, ElementRef, HostListener } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ThemeService } from '../../services/theme.service';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-navbar',
  imports: [MatButtonModule, MatIconModule, MatInputModule, MatFormFieldModule, RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  constructor(
    private theme: ThemeService,
    private elementRef: ElementRef
  ) { }

  toggleDarkMode() {
    this.theme.toggleTheme();
  }
  mobileMenuOpen = false;

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const clickedInside = this.elementRef.nativeElement.contains(event.target);
    if (!clickedInside) {
      this.mobileMenuOpen = false;
    }
  }
}
