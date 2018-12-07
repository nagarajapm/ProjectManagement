import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { ProjectData } from '../_models/project.data';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private apiUrl: string = 'http://localhost:57022/api/projects';

  constructor(private http: HttpClient) { }

  addProject(request: any): Observable<ProjectData> {
    return this.http.post<ProjectData>(this.apiUrl, request);
  }

  getProjects(): Observable<ProjectData[]> {
    return this.http.get<ProjectData[]>(this.apiUrl);
  }

  deleteProject(id: number): Observable<boolean> {
    return this.http.delete<boolean>(this.apiUrl + '/' + id);
  }

  getProject(id: number): Observable<ProjectData> {
    return this.http.get<ProjectData>(this.apiUrl + '/' + id);
  }

  updateProject(id: number, request: any): Observable<ProjectData> {
    return this.http.put<ProjectData>(this.apiUrl + '/' + id, request);
  }
}
