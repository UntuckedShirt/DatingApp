import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

// below is a decorator
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
  
  // HttpClient performs http requests and is an 
  // injectable class for http requests
  // Angular comes with lifecycle events. These events // take p[lace after the instructor known as
  // initialiazation
export class AppComponent implements OnInit{
  // class property below
  title = 'The Dating App';
  // any keyword tyurns off type safety in typescript

  users: any;

  constructor(private http: HttpClient)
  {


  }
  ngOnInit() {
    this.getUsers();
  }

  getUsers() {
    // when we want to use our service we use the this 
    // keyword when accessing and propertry inside a
    // class. http and title is apaert of the class
    // An Obseravable is used in async in angular
    // below is the URL. In order to make it do somehting
    // we need to use subscribe
    // we need to set a param of reponse. This response will contain the users
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: error => console.log(error)
    })
  }
}

