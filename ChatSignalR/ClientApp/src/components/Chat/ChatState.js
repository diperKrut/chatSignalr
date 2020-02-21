import React, { useEffect, useState } from "react";
import {ChatContext} from "./ChatContext";
import {TokenLocal} from "../../TokenLocal/TokenLocal";
import {HandlerExpiredError} from "../../TokenLocal/HandlerError";
import {Redirect} from "react-router-dom";
import {InitiliazeData} from "../../TokenLocal/requests";

export const ChatState=({children})=>{
    
    let [hub,setHub]= useState(null);
    let [data, setData]= useState({
        login:"",
        user:[],
        massages:[]
    });
    let [isLoad, setLoad]=useState(null);
    let [isOk, setIsOk]=useState(true);
    const initHub=async()=>{
        if(!TokenLocal.getIsAuth){
            return;
        }
        let ConnectHub= InitiliazeData.getChatHub();
        ConnectHub.start();
        setHub(ConnectHub); 
    }
    const initData=async()=>{
        let res= await InitiliazeData.getData();
            setData({
                login: res.login,
                user: res.users,
                massages: res.massages
            })      
    }

    const init=async()=>{
        try{
            setLoad(true)
            await initData();
            await initHub();
            setLoad(false);
            if(!isOk) setIsOk(true);
        }
        catch(err){
            setIsOk(false)
            HandlerExpiredError(err,init);
        }
    }
    useEffect(()=>{
        init();       
    },[])
    useEffect(()=>{
        if(hub===null|| isLoad) return;
        hub.on("Send",(massage)=>{
            let massages= data.massages.concat([massage]);
            setData({user: data.user, massages:massages, login: data.login});

        });
        const UpdateUsers=(userUpdate)=>{
           let users= data.user.map(el=>{
               el.isOnLine=el.id===userUpdate.id? userUpdate.isOnLine: el.isOnLine;
               return el;
           })
           setData({user: users, massages:data.massages, login: data.login});           
        }

        hub.on("OffLine",(user)=>{
            UpdateUsers(user)
        });

        hub.on("Online",(user)=>{
            UpdateUsers(user);
        });

        hub.on("Delete", (id)=>{
            var massagesUpd= data.massages.filter(el=>el.id!==id);
            setData({user: data.user, massages:massagesUpd, login: data.login })
        })

        hub.on("Change", (id, newContent)=>{
            var massagesUpd= data.massages.map(el=>{
                el.content= el.id===id? newContent: el.content;
                return el;
            })
            setData({user: data.user, massages:massagesUpd, login: data.login})
        })

        hub.on("NewUser",(user)=>{
            let users= data.user.concat([user]);
            setData({user:users, massages: data.massages, login: data.login} )
        })
        return function cleanup() {
            hub.off("Send");
            hub.off("Online");
            hub.off("OffLine");
            hub.off("Delete");
            hub.off("Change");
            hub.off("NewUser");
          };
    })
    const InvokeHub=(name,mes,type)=>{       
        if(hub===null) return;
        if(type==="SEND_MASSAGE"){
            hub.invoke(name, mes);
        }
        else if(type==="DELETE_MASSAGE"){
            hub.invoke(name, mes);
        }
        else if(type==="CHANGE_MASSAGE"){
            let [id, newContent]= mes;
            hub.invoke(name, id,newContent);
        }   
    }
 

    if(!(isOk || TokenLocal.getIsAuth)){
        return <Redirect to={"/login"} />
    }
    return(
        <ChatContext.Provider value={{InvokeHub,data,setData,isLoad}}>
            {children}
        </ChatContext.Provider>
    )

}