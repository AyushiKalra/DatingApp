import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'app/_models/member';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'environments/environment';
import { User } from 'app/_models/user';
import { AccountService } from 'app/_services/account.service';
import { take } from 'rxjs';
import { Photo } from 'app/_models/photo';
import { MembersService } from 'app/_services/members.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
@Input() member  : Member | undefined;
uploader : FileUploader | undefined;
hasBaseDropZoneOver = false;
baseUrl = environment.apiUrl;
user : User | undefined;

  constructor(private accountService : AccountService, private memberService : MembersService) {  
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next : user => {
        if(user) this.user = user;
      }
    })
  }
  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any){
    this.hasBaseDropZoneOver = e;
  }
  setMainPhoto(photo : Photo)
  {
    this.memberService.setMainPhoto(photo.id).subscribe({
      next: () =>{//() => because we are not getting any content back from API.
        if( this.user && this.member){
          this.user.photoUrl = photo.url;
          this.accountService.setCurrentUser(this.user);
          //the above will allow the updated user(with url) to other components who are subscribing to this service.
          this.member.photoUrl = photo.url;
          this.member.photos.forEach(p=>{
            if(p.isMain) p.isMain = false;
            if(p.id == photo.id) p.isMain = true;
          })
        }
      }
    })
  }
  deletePhoto(photoId : number){
    return this.memberService.deletePhoto(photoId).subscribe({
      next : () =>{
        if(this.member){
          this.member.photos = this.member.photos.filter(x=> x.id != photoId);
          //removing that one photo from photos array by using filter method.
        }
      }
    })
  }
  initializeUploader(){
    this.uploader = new FileUploader({
      url : this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user?.token,
      isHTML5 : true,
      allowedFileType : ['image'],
      removeAfterUpload: true,
      autoUpload : false,
      maxFileSize : 1 * 1024 *1024
    });
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false //if we don't specify this then we have to readjust our CORS policy
    }
    this.uploader.onSuccessItem = (item, respone, status, headers) =>{
      if(respone){
        const photo =JSON.parse(respone);
        this.member?.photos.push(photo);
      }    
    }
  }
}
