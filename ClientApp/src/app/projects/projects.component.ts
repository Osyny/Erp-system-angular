
import { Component, Inject, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Observable, SubscriptionLike } from "rxjs";
import { AuthorizeService, IUser } from "src/api-authorization/authorize.service";
import { IProject, ProjectsService, ProjectVm } from "../services/projects.service";

@Component({  
    selector: 'app-projects',  
    templateUrl: './projects.component.html',
    styleUrls: ['./projects.component.css']
  })  
  export class ProjectsComponent  implements OnInit  {
      progectsVm: IProject | undefined;
      subscriptions: SubscriptionLike[] = [];
      projects: ProjectVm[] = [];
      isAuthenticated: Observable<boolean>;
      public userName: IUser | undefined;

      constructor(private progectsService: ProjectsService,
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private authorizeService: AuthorizeService,
        ) {
      }
      
    ngOnInit(): void {
     this.getProjects();
    }

    getProjects() {

      this.subscriptions.push(this.authorizeService.getUser().subscribe((user) => {

        if(user) 
          this.userName = user;
 
        let userName = this.userName!==undefined ? this.userName!.name : "empty";
        this.subscriptions.push(this.progectsService.getProgects(userName)
        .subscribe(res => {
  
            if(!res) return;
  
            this.isAuthenticated = this.authorizeService.isAuthenticated();
            this.progectsVm = res;
            this.projects = this.progectsVm.projectsVm;
        }))
    }));
    }

    async clickAddNewProject() {
        await this.router.navigate(['/addNewProject'], {
            relativeTo: this.activatedRoute,      
          })
     }

     async clickEdit(Id: number) {
      await this.router.navigate(['/editProjects'], {
        relativeTo: this.activatedRoute,    
        queryParams: {
          projectId: Id,
        }  
      })
     }

     clickDelate(id: number) {
      this.subscriptions.push(this.progectsService.deleteproject(id)
      .subscribe(res => {

        this.getProjects();
      }))
     }

    async redirectToProject(id: number) {
      await this.router.navigate(['/aboutProject'], {
        relativeTo: this.activatedRoute,    
        queryParams: {
          projectId: id,
        } 
      }) 
     }

    ngOnDestroy(): void {
        this.subscriptions.forEach(
          (subscription) => subscription.unsubscribe());
      }
}