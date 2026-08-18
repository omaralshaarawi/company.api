import { Routes } from '@angular/router';
import { AssetFormComponent } from './asset-form/asset-form.component';
import { AssetListComponent } from './asset-list/asset-list.component';
import { AssetHistoryComponent } from './asset-history/asset-history.component';

export const ASSETS_ROUTES: Routes = [  
    { path: '', component: AssetListComponent },
    { path: 'new', component: AssetFormComponent },
    { path: ':id/edit', component: AssetFormComponent },
    { path: ':id/history', component: AssetHistoryComponent }
];