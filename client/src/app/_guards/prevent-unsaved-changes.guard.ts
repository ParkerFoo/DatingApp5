import { compileComponentFromMetadata } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(component: MemberEditComponent):  boolean {
    if (component.editForm.dirty){
      return confirm('Are you sure you want to continue? Any unsaved changes will be lost'); //if user choose no, it will return false which means he will stay on the page
    }
    return true; //if component is not dirty, user can move away from the page.
  }
  
}
