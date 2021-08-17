import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, pipe } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';


// const httpOptions = {
//   headers:new HttpHeaders({
//     Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token//? is the optional chaining just in case
//   })
// }

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache=new Map(); 
  user:User;
  userParams:UserParams; 

  constructor(private http: HttpClient,private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user=>{
      this.user=user;
      this.userParams=new UserParams(user);
    })
   }

   getUserParams(){
    return this.userParams;
   }

   setUserParams(params: UserParams){
      this.userParams=params;
   }

   resetUserParams(){
     this.userParams=new UserParams(this.user); //renew the userParams to its initial state
     return this.userParams
   }

 // getMembers(page?: number, itemsPerPage?: number) {
  getMembers(userParams: UserParams) {
    console.log(Object.values(userParams).join('-'));

    var response=this.memberCache.get(Object.values(userParams).join('-'));
    console.log('member cache is  ' + this.memberCache);    
    console.log(this.memberCache);
    if(response){
      return of(response);
    }

    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);
    //if members array of this service is not 0, return this array as an observable 
    //if (this.members.length > 0) return of(this.members); //of is a rxjs operator which will return as observable
    //get from API and append the values into member array and return it
    return this.getPaginatedResult<Member[]>(this.baseUrl+'users',params).
      pipe(map(response=>{
      this.memberCache.set(Object.values(userParams).join('-'),response);
      return response;
    }))
  }

  private getPaginatedResult<T>(url, params: HttpParams) {
    const paginatedResult: PaginatedResult <T>= new PaginatedResult<T>();
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      // map(members => {
      //   this.members = members;
      //   return members;
      // })
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  private getPaginationHeaders(pageNumber:number,pageSize:number){
    let params = new HttpParams();
  
      params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());

    return params;
    
  }

  getMember(username: string): Observable<Member> {
    const member=[...this.memberCache.values()]
    .reduce((arr,elem)=>arr.concat(elem.result),[])  //reduce it to get only the users array instead of pagination array while at the same time combine multiple array of result into 1
    .find((member:Member)=>member.username===username);//get the clicked user's username
    console.log(member);
    if (member){
      return of(member); //Returns an Observable instance that synchronously delivers the values provided as arguments.
    }

    // const member=this.members.find(x=>x.username===username);
    // if(member!==undefined) return of(member);

    console.log(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    return this.http.put<Member>(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }

  setMainPhoto(photoId: number){
    return this.http.put(this.baseUrl + 'users/set-main-photo/'+ photoId, {});
  }

  deletePhoto(photoId:number){
    return this.http.delete(this.baseUrl + 'users/delete-photo/'+photoId);
  }


}
