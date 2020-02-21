import React, { useContext,useRef } from 'react';
import {ChatContext} from "./Chat/ChatContext";
import "./css/style.css"
import {Massage} from "./Massage";
import {UserOnline} from "./UserOnline";


export const Chat=()=>{
    let context= useContext(ChatContext)
    let inputRef= useRef(null);
    
    const handlerInput=(e)=>{
        if(e.charCode==13){
            context.InvokeHub("Send",inputRef.current.value,"SEND_MASSAGE");
            inputRef.current.value="";
        }
    }

    let check= context.isLoad? <p>Загрузка</p>:
    <div>
        <div className={"UserList"}>
            <p>Учасники</p>
            {context.data.user.map(el=><UserOnline key={el.id} Login={el.login}  IsOnline={el.isOnLine} />)}
        </div>
        <div className={"massagesChat"}>
            
            {context.data.massages.map(el=><Massage key={el.id} id={el.id} CurrentLogin={context.data.login} From={el.userLogin} Content={el.content} />)}
        </div>
        <div>
            <input type={"text"} ref={inputRef} onKeyPress={handlerInput} placeholder={"Введите сообщение"}></input>           
           
        </div>
    </div>
    

    return(
        <div className={"chat"}>
            {check}            
        </div>
    )
}


