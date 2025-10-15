import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TitlesListComponent } from './components/titles-list/titles-list.component';

const routes: Routes = [
  { path: '', component: TitlesListComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TitlesRoutingModule { }
