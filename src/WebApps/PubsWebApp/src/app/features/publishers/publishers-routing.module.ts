import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PublishersListComponent } from './components/publishers-list/publishers-list.component';

const routes: Routes = [
  { path: '', component: PublishersListComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublishersRoutingModule { }
