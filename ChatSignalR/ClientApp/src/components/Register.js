import React,{useRef, useState} from 'react';
import axios from "axios";
import {TokenLocal} from "../TokenLocal/TokenLocal";
import {Link, Redirect} from "react-router-dom";
import {ErrorList} from "./ErrorList";


export const Register=()=>{


    let [errors, setErros]= useState({
        isError: false,
        errors:[]
    })
    let [isOk, setIsOk]=useState(false);
    let refLogin=useRef(null);
    let refPassword=useRef(null);
    let refRePassword=useRef(null);


    const handler=async()=>{
        try{
            let login= refLogin.current.value;
            let password= refPassword.current.value;
            let rePassword= refRePassword.current.value;
            const DataToSend={
                Login:login,
                Password: password,
                RePassword:rePassword
            }
            let res= await axios.post("https://localhost:44302/account/register", DataToSend);
            TokenLocal.SetToken(res.data.jwt);
            TokenLocal.SetRefreshToken(res.data.refreshToken);
            TokenLocal.setIsAuth(true);
            setErros({isError:false})
            setIsOk(true);
        }
        catch(err){
            console.log(err.response.data);
            const errorMessage=(arr)=>{
                return arr.map(el=> el.errorMessage)
            }

            let errors= err.response.data.erros;
            if(errors){                   
                let errorsLogin= errors.Login? errorMessage(errors.Login.errors): [];
                let errorsPassword= errors.Password? errorMessage(errors.Password.errors): [];
                let errorsRePassword= errors.RePassword? errorMessage(errors.RePassword.errors): [];
                setErros({
                    isError:true,
                    errors:[...errorsPassword, ...errorsLogin, ...errorsRePassword]
                })
                console.log([...errorsPassword, ...errorsLogin, ...errorsRePassword]);
            }
            else{
                let errLogin= err.response.data.LoginExist;
                setErros({
                    isError:true,
                    errors:[errLogin]
                })
            }
           
        }
    }
    

    if(isOk){
        return <Redirect to="chatReact"/>
    }

    let ViewError= errors.isError?
    <div>
        <ErrorList errors={errors.errors}/>
    </div>:null

    return(
    <div className="form">
            {ViewError}
		    <div className="comp">
                <div className="dispInline">
                    <div className="signIn activity">Регестрация</div>
                    <div className="signUp"><Link to="/login">Войти</Link></div>
		        </div>
		    <div>
                <input type="text" placeholder="Введите логин" ref={refLogin}/>
                <br />
                <input type="password" placeholder="Введите пароль" ref={refPassword}/>
                <input type="password" placeholder="Повторите ваш пароль" ref={refRePassword}/>
                <button className="button" onClick={handler}>Зарегестироваться</button>
            </div>
        </div>
    </div>
    )
} 