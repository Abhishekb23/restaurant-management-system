import {
  Component
} from '@angular/core';

@Component({
  selector: 'app-dashboard',

  standalone: true,

  template: `
    <div
      class="
        min-h-screen
        flex
        items-center
        justify-center
        bg-black
        text-white
        text-5xl
        font-bold
      "
    >
      Dashboard
    </div>
  `
})
export class Dashboard {}