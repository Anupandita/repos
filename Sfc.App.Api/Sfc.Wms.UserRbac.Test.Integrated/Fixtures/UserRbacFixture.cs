using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Contracts.Dtos;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.UserRbac.Test.Integrated.TestData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Sfc.Wms.UserRbac.Test.Integrated.Fixtures
{
    public class UserRbacFixture : DataBaseFixture
    {
        protected LoginTestData LoginSource;
        protected UrlTestData UrlData;
        protected Urls Url;
        protected string baseUrl = "http://localhost:59665/user/";        
        private readonly int count;
        protected int menuCount;
        protected int permissionCount;
        protected int preferenceCount;
        protected List<string> menusIds;
        protected List<string> permissionsIds;
        protected List<string> preferencesIds;       
        protected string currentUrl;        

        protected void AValidNewLoginRecord()
        {
            LoginSource = new LoginTestData();
            LoginSource.UserName = "PSI";
            LoginSource.Password = "WOLF";
        }

        protected void AValidUrlTestData()
        {
            UrlData = new UrlTestData();
            UrlData.Login = "login";
            UrlData.Menus = "menus";
            UrlData.Permissions = "permissions";
            UrlData.Preferences = "preferences";
        }

        protected void AValidRbacUrls()
        {
            Url = new Urls();
            Url.Login = $"{baseUrl}{nameof(UrlData.Login)}";
            Url.Menus = $"{baseUrl}{nameof(UrlData.Menus)}";
            Url.Permissions = $"{baseUrl}{nameof(UrlData.Permissions)}";
            Url.Preferences = $"{baseUrl}{nameof(UrlData.Preferences)}";
        }

        protected void SetCurrentUrlToMenu()
        {
            currentUrl = Url.Menus;
        }

        protected void SetCurrentUrlToPermission()
        {
            currentUrl = Url.Permissions;
        }

        protected void SetCurrentUrlToPreference()
        {
            currentUrl = Url.Preferences;
        }
        
        protected void UserRbacTestData()
        {
            GetDataFromDatabase();
        }
       
        protected IRestResponse SignInIsCalledOkIsReturned()
        {
         
            var client = new RestClient(Url.Login);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.AddJsonBody(LoginSource);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);          
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            return response;
        }

        protected string ReadTokenValue()
        {
            var response = SignInIsCalledOkIsReturned();
            var t = JsonConvert.DeserializeObject<Result.BaseResult<UserInfoDto>>(response.Content.ToString());
            var authToken = t.Payload.Token;
            return authToken;
        }

        protected IRestResponse UserRbacApiIsCalledOkIsReturned()
        {
            var token = ReadTokenValue();
            var client = new RestClient(currentUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader(Headers.Authorization, Headers.Bearer + token);
            var response = client.Execute(request);          
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            return response;
        }

        protected void ReadTheMenusFromLoginResult()
        {
            var response = SignInIsCalledOkIsReturned();
            var menus = JsonConvert.DeserializeObject<Core.OnPrem.Result.BaseResult<UserInfoDto>>(response.Content.ToString());          
            menusIds = new List<string>();
            foreach (var m in menus.Payload.Menus)
            {              
                menusIds.Add(m.MenuId.ToString());             
                foreach (var c in m.ChildMenus ?? new List<MenuDetailsDto>())
                {                   
                    menusIds.Add(c.MenuId.ToString());                  
                }
            }
            menuCount = menusIds.Count;
        }

        protected void ReadTheMenusFromMenuResult()
        {
            var response = UserRbacApiIsCalledOkIsReturned();
            var menus = JsonConvert.DeserializeObject<Core.OnPrem.Result.BaseResult<List<MenusDto>>>(response.Content.ToString());
            menusIds = new List<string>();
            foreach (var m in menus.Payload)
            {
                menusIds.Add(m.MenuId.ToString());
                foreach (var c in m.ChildMenus.Select(c => c.MenuId))
                {
                    menusIds.Add(c.ToString());
                }
            }
            menuCount = menusIds.Count;
        }

        protected void ReadThePermissionsFromLoginResult()
        {
            var response = SignInIsCalledOkIsReturned();
            var permission = JsonConvert.DeserializeObject<Core.OnPrem.Result.BaseResult<UserInfoDto>>(response.Content.ToString());
           
            permissionsIds = new List<string>();
            foreach (var p in permission.Payload.Permissions.Select(p=>p.PermissionId))
            {              
                permissionsIds.Add(p.ToString());              
            }
            permissionCount = permissionsIds.Count;
        }

        protected void ReadThePermissionsFromPermissionResult()
        {
            var response = UserRbacApiIsCalledOkIsReturned();         
            var permissions = JsonConvert.DeserializeObject<Core.OnPrem.Result.BaseResult<List<PermissionsDto>>>(response.Content.ToString());
            permissionCount = permissions.Payload.Count;
            permissionsIds = new List<string>();
            foreach (var p in permissions.Payload.Select(p=>p.PermissionId))
            {                
                permissionsIds.Add(p.ToString());              
            }           
        }      
     
        protected void ReadThePreferencesFromLoginResult()
        {
            var response = SignInIsCalledOkIsReturned();
            var preferences = JsonConvert.DeserializeObject<Core.OnPrem.Result.BaseResult<UserInfoDto>>(response.Content.ToString());
            preferenceCount = preferences.Payload.Preferences.Count;           
            preferencesIds = new List<string>();
            foreach (var pr in preferences.Payload.Preferences.Select(pr=>pr.SettingId))
            {              
                preferencesIds.Add(pr.ToString());
            }
           
        }

        protected void ReadThePreferencesFromPreferenceResult()
        {
            var response = UserRbacApiIsCalledOkIsReturned();
            var preferences = JsonConvert.DeserializeObject<Core.OnPrem.Result.BaseResult<List<PreferencesDto>>>(response.Content.ToString());
            preferenceCount = preferences.Payload.Count;            
            preferencesIds = new List<string>();           
            foreach (var pr in preferences.Payload.Select(pr=>pr.SettingId))
            {              
                preferencesIds.Add(pr.ToString());
            }        
        }
       
        
        protected void ValidateUserAssociatedMenuCounts()
        {
            Assert.AreEqual(dbMenuCount, menuCount);
        }

       
        protected void ValidateUserAssociatedMenuIdValues()
        {      
                foreach (var menuList in menusIds)
                {                                           
                    Assert.IsTrue(dbMenus.Contains(menuList), $"{menuList} doesn't exist.");                                            
                }               
           
        }
       
        protected void ValidateUserAssociatedPermissionCounts()
        {
            Assert.AreEqual(dbPermissionCount, permissionCount);
        }

       
        protected void ValidateUserAssociatedPermissionIdValues()
        {            
                foreach (var permissionList in permissionsIds)
                {                   
                    Assert.IsTrue(dbPermissions.Contains(permissionList), $"{permissionList} doesn't exist.");
                }
        }

         
        protected void ValidateUserAssociatedPreferenceCounts()
        {
            Assert.AreEqual(dbPreferenceCount, preferenceCount);
        }

       
        protected void ValidateUserAssociatedPreferenceIdValues()
        {                         
                foreach (var preferenceList in preferencesIds)
                {                    
                    Assert.IsTrue(dbPreferences.Contains(preferenceList), $"{preferenceList} doesn't exist.");
                } 
                
        }

    }
}

