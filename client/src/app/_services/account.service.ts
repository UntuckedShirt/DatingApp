import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';


// to create this file you would type ng g 
// s account --skip-tests. This file woiuld // be used for login accounts
// Injectable decorators means this service // an be injected into other componenets or // services in aour applcation
// services are injectable
@Injectable({
  // we have meta-data here(providedIn)
  // an Angular service is a singlton, when // we injecrt it into a complenent and
  // initialized itll stay initialie ill
  // app is closed by user
  providedIn: 'root'
})
  // below is used to make requests inside 
  // the API. we give it a baseUrl proprty
  // which is set to assign a url
export class AccountService {
  baseUrl = 'https://localhost:5001/api/'; 
  // here we will reate an observable
  // this special observable(ReplaySubject) is a buffer obj
  // that stores the values. When any sub(user) subscribes
  // to the  observable itll emit hte last value insdie it
  // or however many values insdie it
  // the 1 is the siuze of the buffer or how many current
  // users we are storing
  private currentUserSource = new ReplaySubject<User>(1)
  currentUser$ = this.currentUserSource.asObservable();
  // we then inject http client into our aacoutn service
  constructor(private http: HttpClient) { }
  // now create login method
  // this will receieve cred from login
  // form form nav-bar


  login(model: any) {
    return this.http.post(this.baseUrl +
      // anyhting within this pipe will use an // RxJ operator
      // well use map function in here
      // we stick with any until typscript
      // if a period is added afte the response you can see
      // our 2 properties
      'account/login', model).pipe(
        map((response: User) => 
        {
          const user = response;
          if (user)
          {
            localStorage.setItem('user', JSON.stringify(user));
            this.currentUserSource.next(user);
            }
        })
    )
  }
  // her will be a help method
  setCurrentUser(user: User)
  {
    this.currentUserSource.next(user);
  }
  // now we need to use typescript and create folder
  logout()
  {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
