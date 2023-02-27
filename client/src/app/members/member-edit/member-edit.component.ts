import { NgFor } from '@angular/common';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Member } from 'app/_models/member';
import { User } from 'app/_models/user';
import { AccountService } from 'app/_services/account.service';
import { MembersService } from 'app/_services/members.service';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
//our goal here is to fetch the member profile so that they can edit. We can do this if we have access to their username.
member : Member | undefined;
user : User | null = null;
@ViewChild('editForm') editForm : NgForm | undefined;//to access the child template's form
@HostListener('window:beforeunload',['$event']) unloadNotification($event:any){
  if(this.editForm?.dirty){
    $event.returnValue = true;
  }
}

constructor(private accountService : AccountService , private memberService : MembersService,
  private toastr : ToastrService) {
  this.accountService.currentUser$.pipe(take(1)).subscribe({
    next : user => this.user = user
  })
}

  ngOnInit(): void {
    this.loadMember();
  }
loadMember(){
  if(!this.user) return;
  this.memberService.getMember(this.user.username).subscribe({
    next : member => this.member = member
  })
}
updateMember(){
  this.memberService.updateMember(this.editForm?.value).subscribe({
    next : _ => {
      this.toastr.success("Profile updated successfully");
      this.editForm?.reset(this.member);
    }
  })
}

}
