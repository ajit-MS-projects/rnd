/*! umbraco - v# - 2014-11-26
 * https://github.com/umbraco/umbraco-cms/
 * Copyright (c) 2014 Umbraco HQ;
 * Licensed MIT
 */

(function() { 

angular.module("umbraco.mocks", ['ngCookies']);
angular.module("umbraco.mocks.services", []);
angular.module('umbraco.mocks').
  factory('prevaluesMocks', ['$httpBackend', 'mocksUtils', function ($httpBackend, mocksUtils) {
      'use strict';
      
      function getRichTextConfiguration(status, data, headers) {
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }
          else {
              return [200, { "plugins": [{ "name": "code", "useOnFrontend": true }, { "name": "paste", "useOnFrontend": true }, { "name": "umbracolink", "useOnFrontend": true }], "commands": [{ "icon": "images/editor/code.gif", "command": "code", "alias": "code", "userInterface": "false", "frontEndCommand": "code", "value": "", "priority": 1, "isStylePicker": false }, { "icon": "images/editor/removeformat.gif", "command": "removeformat", "alias": "removeformat", "userInterface": "false", "frontEndCommand": "removeformat", "value": "", "priority": 2, "isStylePicker": false }, { "icon": "images/editor/undo.gif", "command": "undo", "alias": "undo", "userInterface": "false", "frontEndCommand": "undo", "value": "", "priority": 11, "isStylePicker": false }, { "icon": "images/editor/redo.gif", "command": "redo", "alias": "redo", "userInterface": "false", "frontEndCommand": "redo", "value": "", "priority": 12, "isStylePicker": false }, { "icon": "images/editor/cut.gif", "command": "cut", "alias": "cut", "userInterface": "false", "frontEndCommand": "cut", "value": "", "priority": 13, "isStylePicker": false }, { "icon": "images/editor/copy.gif", "command": "copy", "alias": "copy", "userInterface": "false", "frontEndCommand": "copy", "value": "", "priority": 14, "isStylePicker": false }, { "icon": "images/editor/showStyles.png", "command": "styleselect", "alias": "styleselect", "userInterface": "false", "frontEndCommand": "styleselect", "value": "", "priority": 20, "isStylePicker": false }, { "icon": "images/editor/bold.gif", "command": "bold", "alias": "bold", "userInterface": "false", "frontEndCommand": "bold", "value": "", "priority": 21, "isStylePicker": false }, { "icon": "images/editor/italic.gif", "command": "italic", "alias": "italic", "userInterface": "false", "frontEndCommand": "italic", "value": "", "priority": 22, "isStylePicker": false }, { "icon": "images/editor/underline.gif", "command": "underline", "alias": "underline", "userInterface": "false", "frontEndCommand": "underline", "value": "", "priority": 23, "isStylePicker": false }, { "icon": "images/editor/strikethrough.gif", "command": "strikethrough", "alias": "strikethrough", "userInterface": "false", "frontEndCommand": "strikethrough", "value": "", "priority": 24, "isStylePicker": false }, { "icon": "images/editor/justifyleft.gif", "command": "justifyleft", "alias": "justifyleft", "userInterface": "false", "frontEndCommand": "alignleft", "value": "", "priority": 31, "isStylePicker": false }, { "icon": "images/editor/justifycenter.gif", "command": "justifycenter", "alias": "justifycenter", "userInterface": "false", "frontEndCommand": "aligncenter", "value": "", "priority": 32, "isStylePicker": false }, { "icon": "images/editor/justifyright.gif", "command": "justifyright", "alias": "justifyright", "userInterface": "false", "frontEndCommand": "alignright", "value": "", "priority": 33, "isStylePicker": false }, { "icon": "images/editor/justifyfull.gif", "command": "justifyfull", "alias": "justifyfull", "userInterface": "false", "frontEndCommand": "alignfull", "value": "", "priority": 34, "isStylePicker": false }, { "icon": "images/editor/bullist.gif", "command": "bullist", "alias": "bullist", "userInterface": "false", "frontEndCommand": "bullist", "value": "", "priority": 41, "isStylePicker": false }, { "icon": "images/editor/numlist.gif", "command": "numlist", "alias": "numlist", "userInterface": "false", "frontEndCommand": "numlist", "value": "", "priority": 42, "isStylePicker": false }, { "icon": "images/editor/outdent.gif", "command": "outdent", "alias": "outdent", "userInterface": "false", "frontEndCommand": "outdent", "value": "", "priority": 43, "isStylePicker": false }, { "icon": "images/editor/indent.gif", "command": "indent", "alias": "indent", "userInterface": "false", "frontEndCommand": "indent", "value": "", "priority": 44, "isStylePicker": false }, { "icon": "images/editor/link.gif", "command": "link", "alias": "mcelink", "userInterface": "true", "frontEndCommand": "link", "value": "", "priority": 51, "isStylePicker": false }, { "icon": "images/editor/unLink.gif", "command": "unlink", "alias": "unlink", "userInterface": "false", "frontEndCommand": "unlink", "value": "", "priority": 52, "isStylePicker": false }, { "icon": "images/editor/anchor.gif", "command": "anchor", "alias": "mceinsertanchor", "userInterface": "false", "frontEndCommand": "anchor", "value": "", "priority": 53, "isStylePicker": false }, { "icon": "images/editor/image.gif", "command": "image", "alias": "mceimage", "userInterface": "true", "frontEndCommand": "umbmediapicker", "value": "", "priority": 61, "isStylePicker": false }, { "icon": "images/editor/insMacro.gif", "command": "umbracomacro", "alias": "umbracomacro", "userInterface": "true", "frontEndCommand": "umbmacro", "value": "", "priority": 62, "isStylePicker": false }, { "icon": "images/editor/table.gif", "command": "table", "alias": "mceinserttable", "userInterface": "true", "frontEndCommand": "table", "value": "", "priority": 63, "isStylePicker": false }, { "icon": "images/editor/media.gif", "command": "umbracoembed", "alias": "umbracoembed", "userInterface": "true", "frontEndCommand": "umbembeddialog", "value": "", "priority": 66, "isStylePicker": false }, { "icon": "images/editor/hr.gif", "command": "hr", "alias": "inserthorizontalrule", "userInterface": "false", "frontEndCommand": "hr", "value": "", "priority": 71, "isStylePicker": false }, { "icon": "images/editor/sub.gif", "command": "sub", "alias": "subscript", "userInterface": "false", "frontEndCommand": "sub", "value": "", "priority": 72, "isStylePicker": false }, { "icon": "images/editor/sup.gif", "command": "sup", "alias": "superscript", "userInterface": "false", "frontEndCommand": "sup", "value": "", "priority": 73, "isStylePicker": false }, { "icon": "images/editor/charmap.gif", "command": "charmap", "alias": "mcecharmap", "userInterface": "false", "frontEndCommand": "charmap", "value": "", "priority": 74, "isStylePicker": false }], "validElements": "+a[id|style|rel|rev|charset|hreflang|dir|lang|tabindex|accesskey|type|name|href|target|title|class|onfocus|onblur|onclick|ondblclick|onmousedown|onmouseup|onmouseover|onmousemove|onmouseout|onkeypress|onkeydown|onkeyup],-strong/-b[class|style],-em/-i[class|style],-strike[class|style],-u[class|style],#p[id|style|dir|class|align],-ol[class|reversed|start|style|type],-ul[class|style],-li[class|style],br[class],img[id|dir|lang|longdesc|usemap|style|class|src|onmouseover|onmouseout|border|alt=|title|hspace|vspace|width|height|align|umbracoorgwidth|umbracoorgheight|onresize|onresizestart|onresizeend|rel],-sub[style|class],-sup[style|class],-blockquote[dir|style|class],-table[border=0|cellspacing|cellpadding|width|height|class|align|summary|style|dir|id|lang|bgcolor|background|bordercolor],-tr[id|lang|dir|class|rowspan|width|height|align|valign|style|bgcolor|background|bordercolor],tbody[id|class],thead[id|class],tfoot[id|class],#td[id|lang|dir|class|colspan|rowspan|width|height|align|valign|style|bgcolor|background|bordercolor|scope],-th[id|lang|dir|class|colspan|rowspan|width|height|align|valign|style|scope],caption[id|lang|dir|class|style],-div[id|dir|class|align|style],-span[class|align|style],-pre[class|align|style],address[class|align|style],-h1[id|dir|class|align],-h2[id|dir|class|align],-h3[id|dir|class|align],-h4[id|dir|class|align],-h5[id|dir|class|align],-h6[id|style|dir|class|align],hr[class|style],dd[id|class|title|style|dir|lang],dl[id|class|title|style|dir|lang],dt[id|class|title|style|dir|lang],object[class|id|width|height|codebase|*],param[name|value|_value|class],embed[type|width|height|src|class|*],map[name|class],area[shape|coords|href|alt|target|class],bdo[class],button[class],iframe[*]", "inValidElements": "font", "customConfig": { "entity_encoding": "raw", "spellchecker_rpc_url": "GoogleSpellChecker.ashx" } }, null];
          }
      }

      return {
          register: function() {
              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/RichTextPreValue/GetConfiguration'))
                  .respond(getRichTextConfiguration);
          }
      };
  }]);
angular.module('umbraco.mocks').
    factory('mocksUtils', ['$cookieStore', function($cookieStore) {
        'use strict';
         
        //by default we will perform authorization
        var doAuth = true;

        return {
            
            getMockDataType: function(id, selectedId) {
                var dataType = {
                    id: id,
                    name: "Simple editor " + id,
                    selectedEditor: selectedId,
                    availableEditors: [
                        { name: "Simple editor 1", editorId: String.CreateGuid() },
                        { name: "Simple editor 2", editorId: String.CreateGuid() },
                        { name: "Simple editor " + id, editorId: selectedId },
                        { name: "Simple editor 4", editorId: String.CreateGuid() },
                        { name: "Simple editor 5", editorId: String.CreateGuid() },
                        { name: "Simple editor 6", editorId: String.CreateGuid() }
                    ],
                    preValues: [
                          {
                              label: "Custom pre value 1 for editor " + selectedId,
                              description: "Enter a value for this pre-value",
                              key: "myPreVal1",
                              view: "requiredfield"                              
                          },
                            {
                                label: "Custom pre value 2 for editor " + selectedId,
                                description: "Enter a value for this pre-value",
                                key: "myPreVal2",
                                view: "requiredfield"                                
                            }
                    ]

                };
                return dataType;
            },

            /** Creats a mock content object */
            getMockContent: function(id) {
                var node = {
                    name: "My content with id: " + id,
                    updateDate: new Date().toIsoDateTimeString(),
                    publishDate: new Date().toIsoDateTimeString(),
                    createDate: new Date().toIsoDateTimeString(),
                    id: id,
                    parentId: 1234,
                    icon: "icon-umb-content",
                    owner: { name: "Administrator", id: 0 },
                    updater: { name: "Per Ploug Krogslund", id: 1 },
                    path: "-1,1234,2455",
                    allowedActions: ["U", "H", "A"],
                    tabs: [
                    {
                        label: "Child documents",
                        id: 1, 
                        active: true,
                        properties: [                            
                            { alias: "list", label: "List", view: "listview", value: "", hideLabel: true, config:{entityType: "content"} },
                        ]
                    },
                    {
                        label: "Content",
                        id: 2,
                        properties: [
                            { alias: "valTest", label: "Validation test", view: "validationtest", value: "asdfasdf" },
                            { alias: "bodyText", label: "Body Text", description: "Here you enter the primary article contents", view: "rte", value: "<p>askjdkasj lasjd</p>", config: {} },
                            { alias: "textarea", label: "textarea", view: "textarea", value: "ajsdka sdjkds", config: { rows: 4 } },
                            { alias: "media", label: "Media picker", view: "mediapicker", value: "1234,23242,23232,23231", config: {multiPicker: 1} }
                        ]
                    },
                    {
                        label: "Sample Editor",
                        id: 3,
                        properties: [
                            { alias: "datepicker", label: "Datepicker", view: "datepicker", config: { pickTime: false, format: "yyyy-MM-dd" } },
                            { alias: "tags", label: "Tags", view: "tags", value: "" }
                        ]
                    },
                    {
                        label: "This",
                        id: 4,
                        properties: [
                            { alias: "valTest4", label: "Validation test", view: "validationtest", value: "asdfasdf" },
                            { alias: "bodyText4", label: "Body Text", description: "Here you enter the primary article contents", view: "rte", value: "<p>askjdkasj lasjd</p>", config: {} },
                            { alias: "textarea4", label: "textarea", view: "textarea", value: "ajsdka sdjkds", config: { rows: 4 } },
                            { alias: "content4", label: "Content picker", view: "contentpicker", value: "1234,23242,23232,23231" }
                        ]
                    },
                    {
                        label: "Is",
                        id: 5,
                        properties: [
                            { alias: "valTest5", label: "Validation test", view: "validationtest", value: "asdfasdf" },
                            { alias: "bodyText5", label: "Body Text", description: "Here you enter the primary article contents", view: "rte", value: "<p>askjdkasj lasjd</p>", config: {} },
                            { alias: "textarea5", label: "textarea", view: "textarea", value: "ajsdka sdjkds", config: { rows: 4 } },
                            { alias: "content5", label: "Content picker", view: "contentpicker", value: "1234,23242,23232,23231" }
                        ]
                    },
                    {
                        label: "Overflown",
                        id: 6,
                        properties: [
                            { alias: "valTest6", label: "Validation test", view: "validationtest", value: "asdfasdf" },
                            { alias: "bodyText6", label: "Body Text", description: "Here you enter the primary article contents", view: "rte", value: "<p>askjdkasj lasjd</p>", config: {} },
                            { alias: "textarea6", label: "textarea", view: "textarea", value: "ajsdka sdjkds", config: { rows: 4 } },
                            { alias: "content6", label: "Content picker", view: "contentpicker", value: "1234,23242,23232,23231" }
                        ]
                    },
                    {
                        label: "Tab # 7",
                        id: 7,
                        properties: [
                            { alias: "valTest7", label: "Validation test", view: "validationtest", value: "asdfasdf" },
                            { alias: "bodyText7", label: "Body Text", description: "Here you enter the primary article contents", view: "rte", value: "<p>askjdkasj lasjd</p>", config: {} },
                            { alias: "textarea7", label: "textarea", view: "textarea", value: "ajsdka sdjkds", config: { rows: 4 } },
                            { alias: "content7", label: "Content picker", view: "contentpicker", value: "1234,23242,23232,23231" }
                        ]
                    },
                    {
                        label: "Grid",
                        id: 8,
                        properties: [
                        { alias: "grid", label: "Grid", view: "grid", value: "test", hideLabel: true }
                        ]
                    }, {
                        label: "Generic Properties",
                        id: 0,
                        properties: [
                            {
                                label: 'Id',
                                value: 1234,
                                view: "readonlyvalue",
                                alias: "_umb_id"
                            },
                            {
                                label: 'Created by',
                                description: 'Original author',
                                value: "Administrator",
                                view: "readonlyvalue",
                                alias: "_umb_createdby"
                            },
                            {
                                label: 'Created',
                                description: 'Date/time this document was created',
                                value: new Date().toIsoDateTimeString(),
                                view: "readonlyvalue",
                                alias: "_umb_createdate"
                            },
                            {
                                label: 'Updated',
                                description: 'Date/time this document was created',
                                value: new Date().toIsoDateTimeString(),
                                view: "readonlyvalue",
                                alias: "_umb_updatedate"
                            },                            
                            {
                                label: 'Document Type',
                                value: "Home page",
                                view: "readonlyvalue",
                                alias: "_umb_doctype" 
                            },
                            {
                                label: 'Publish at',
                                description: 'Date/time to publish this document',
                                value: new Date().toIsoDateTimeString(),
                                view: "datepicker",
                                alias: "_umb_releasedate"
                            },
                            { 
                                label: 'Unpublish at',
                                description: 'Date/time to un-publish this document',
                                value: new Date().toIsoDateTimeString(),
                                view: "datepicker",
                                alias: "_umb_expiredate"
                            },
                            {
                                label: 'Template', 
                                value: "myTemplate",
                                view: "dropdown",
                                alias: "_umb_template",
                                config: {
                                    items: {
                                        "" : "-- Choose template --",
                                        "myTemplate" : "My Templates",
                                        "home" : "Home Page",
                                        "news" : "News Page"
                                    }
                                }
                            },
                            {
                                label: 'Link to document',
                                value: ["/testing" + id, "http://localhost/testing" + id, "http://mydomain.com/testing" + id].join(),
                                view: "urllist",
                                alias: "_umb_urllist"
                            },
                            {
                                alias: "test", label: "Stuff", view: "test", value: "",
                                config: {
                                    fields: [
                                                { alias: "embedded", label: "Embbeded", view: "textstring", value: "" },
                                                { alias: "embedded2", label: "Embbeded 2", view: "contentpicker", value: "" },
                                                { alias: "embedded3", label: "Embbeded 3", view: "textarea", value: "" },
                                                { alias: "embedded4", label: "Embbeded 4", view: "datepicker", value: "" }
                                    ]
                                }
                            }
                        ]
                    }
                    ]
                };

                return node;
            },

            getMockEntity : function(id){
                return {name: "hello", id: id, icon: "icon-file"};
            },

            /** generally used for unit tests, calling this will disable the auth check and always return true */
            disableAuth: function() {
                doAuth = false;
            },

            /** generally used for unit tests, calling this will enabled the auth check */
            enabledAuth: function() {
                doAuth = true;
            }, 

            /** Checks for our mock auth cookie, if it's not there, returns false */
            checkAuth: function () {
                if (doAuth) {
                    var mockAuthCookie = $cookieStore.get("mockAuthCookie");
                    if (!mockAuthCookie) {
                        return false;
                    }
                    return true;
                }
                else {
                    return true;
                }
            },
            
            /** Creates/sets the auth cookie with a value indicating the user is now authenticated */
            setAuth: function() {
                //set the cookie for loging
                $cookieStore.put("mockAuthCookie", "Logged in!");
            },
            
            /** removes the auth cookie  */
            clearAuth: function() {
                $cookieStore.remove("mockAuthCookie");
            },

            urlRegex: function(url) {
                url = url.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
                return new RegExp("^" + url);
            },

            getParameterByName: function(url, name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");

                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(url);

                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            },

            getParametersByName: function(url, name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");

                var regex = new RegExp(name + "=([^&#]*)", "mg"), results = [];
                var match;

                while ( ( match = regex.exec(url) ) !== null )
                {
                    results.push(decodeURIComponent(match[1].replace(/\+/g, " ")));
                }

                return results;
            }
        };
    }]);

angular.module('umbraco.mocks').
  factory('contentMocks', ['$httpBackend', 'mocksUtils', function ($httpBackend, mocksUtils) {
      'use strict';
      
      function returnChildren(status, data, headers) {
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var pageNumber = Number(mocksUtils.getParameterByName(data, "pageNumber"));
          var filter = mocksUtils.getParameterByName(data, "filter");
          var pageSize = Number(mocksUtils.getParameterByName(data, "pageSize"));
          var parentId = Number(mocksUtils.getParameterByName(data, "id"));

          if (pageNumber === 0) {
              pageNumber = 1;
          }
          var collection = { pageSize: pageSize, totalItems: 68, totalPages: 7, pageNumber: pageNumber, filter: filter };
          collection.totalItems = 56 - (filter.length);
          if (pageSize > 0) {
              collection.totalPages = Math.round(collection.totalItems / collection.pageSize);
          }
          else {
              collection.totalPages = 1;
          }
          collection.items = [];

          if (collection.totalItems < pageSize || pageSize < 1) {
              collection.pageSize = collection.totalItems;
          } else {
              collection.pageSize = pageSize;
          }
          
          var id = 0;
          for (var i = 0; i < collection.pageSize; i++) {
              id = (parentId + i) * pageNumber;
              var cnt = mocksUtils.getMockContent(id);

              //here we fake filtering
              if (filter !== '') {
                  cnt.name = filter + cnt.name;
              }

              //set a fake sortOrder
              cnt.sortOrder = i + 1;

              collection.items.push(cnt);
          }

          return [200, collection, null];
      }

      function returnDeletedNode(status, data, headers) {
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }
          
          return [200, null, null];
      }

      function returnEmptyNode(status, data, headers) {

          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var response = returnNodebyId(200, "", null);
          var node = response[1];
          var parentId = mocksUtils.getParameterByName(data, "parentId") || 1234;

          node.name = "";
          node.id = 0;
          node.parentId = parentId;

          $(node.tabs).each(function(i,tab){
              $(tab.properties).each(function(i, property){
                  property.value = "";
              });
          });

          return response;
      }

      function returnNodebyId(status, data, headers) {

          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var id = mocksUtils.getParameterByName(data, "id") || "1234";
          id = parseInt(id, 10);

          var node = mocksUtils.getMockContent(id);

          return [200, node, null];
      }
      
      function returnNodebyIds(status, data, headers) {

          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var ids = mocksUtils.getParameterByName(data, "ids") || [1234,23324,2323,23424];
          var nodes = [];

          $(ids).each(function(i, id){
            var _id = parseInt(id, 10);
            nodes.push(mocksUtils.getMockContent(_id)); 
          });
          
          return [200, nodes, null];
      }

      function returnSort(status, data, headers) {
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }
          
          return [200, null, null];
      }
      
      function returnSave(status, data, headers) {
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          return [200, null, null];
      }

      return {
          register: function () {

              $httpBackend
                  .whenPOST(mocksUtils.urlRegex('/umbraco/UmbracoApi/Content/PostSave'))
                  .respond(returnSave);

              $httpBackend
                  .whenPOST(mocksUtils.urlRegex('/umbraco/UmbracoApi/Content/PostSort'))
                  .respond(returnSort);

              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Content/GetChildren'))
                  .respond(returnChildren);

              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Content/GetByIds'))
                  .respond(returnNodebyIds);

              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Content/GetById?'))
                  .respond(returnNodebyId);

              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Content/GetEmpty'))
                  .respond(returnEmptyNode);

              $httpBackend
                  .whenDELETE(mocksUtils.urlRegex('/umbraco/UmbracoApi/Content/DeleteById'))
                  .respond(returnDeletedNode);
              
              $httpBackend
                  .whenDELETE(mocksUtils.urlRegex('/umbraco/UmbracoApi/Content/EmptyRecycleBin'))
                  .respond(returnDeletedNode);
          },

          expectGetById: function() {
              $httpBackend
                  .expectGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Content/GetById'));
          }
      };
  }]);
angular.module('umbraco.mocks').
  factory('contentTypeMocks', ['$httpBackend', 'mocksUtils', function ($httpBackend, mocksUtils) {
      'use strict';
      
      function returnAllowedChildren(status, data, headers) {

          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var types = [
                { name: "News Article", description: "Standard news article", alias: "newsArticle", id: 1234, icon: "icon-file", thumbnail: "icon-file" },
                { name: "News Area", description: "Area to hold all news articles, there should be only one", alias: "newsArea", id: 1234, icon: "icon-suitcase", thumbnail: "icon-suitcase" },
                { name: "Employee", description: "Employee profile information page", alias: "employee", id: 1234, icon: "icon-user", thumbnail: "icon-user" }
          ];
          return [200, types, null];
      }

      return {
          register: function() {
              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/Api/ContentType/GetAllowedChildren'))
                  .respond(returnAllowedChildren);
                
          },
          expectAllowedChildren: function(){
            console.log("expecting get");
            $httpBackend.expectGET(mocksUtils.urlRegex('/umbraco/Api/ContentType/GetAllowedChildren'));
          }
      };
  }]);
angular.module('umbraco.mocks').
  factory('dashboardMocks', ['$httpBackend', 'mocksUtils', function ($httpBackend, mocksUtils) {
      'use strict';
      
      function getDashboard(status, data, headers) {
          //check for existence of a cookie so we can do login/logout in the belle app (ignore for tests).
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }
          else {
              //TODO: return real mocked data
              return [200, [], null];
          }
      }

      return {
          register: function() {
              
              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Dashboard/GetDashboard'))
                  .respond(getDashboard);
          }
      };
  }]);
angular.module('umbraco.mocks').
  factory('dataTypeMocks', ['$httpBackend', 'mocksUtils', function ($httpBackend, mocksUtils) {
      'use strict';
      
      function returnById(status, data, headers) {

          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var id = mocksUtils.getParameterByName(data, "id") || 1234;

          var selectedId = String.CreateGuid();

          var dataType = mocksUtils.getMockDataType(id, selectedId);
              
          return [200, dataType, null];
      }
      
      function returnEmpty(status, data, headers) {

          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var response = returnById(200, "", null);
          var node = response[1];

          node.name = "";
          node.selectedEditor = "";
          node.id = 0;
          node.preValues = [];

          return response;
      }
      
      function returnPreValues(status, data, headers) {

          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var editorId = mocksUtils.getParameterByName(data, "editorId") || "83E9AD36-51A7-4440-8C07-8A5623AC6979";

          var preValues = [
              {
                  label: "Custom pre value 1 for editor " + editorId,
                  description: "Enter a value for this pre-value",
                  key: "myPreVal",
                  view: "requiredfield",
                  validation: [
                      {
                          type: "Required"
                      }
                  ]
              },
              {
                  label: "Custom pre value 2 for editor " + editorId,
                  description: "Enter a value for this pre-value",
                  key: "myPreVal",
                  view: "requiredfield",
                  validation: [
                      {
                          type: "Required"
                      }
                  ]
              }
          ];
          return [200, preValues, null];
      }
      
      function returnSave(status, data, headers) {
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var postedData = angular.fromJson(headers);

          var dataType = mocksUtils.getMockDataType(postedData.id, postedData.selectedEditor);
          dataType.notifications = [{
              header: "Saved",
              message: "Data type saved",
              type: 0
          }];

          return [200, dataType, null];
      }

      return {
          register: function() {
              
              $httpBackend
                  .whenPOST(mocksUtils.urlRegex('/umbraco/UmbracoApi/DataType/PostSave'))
                  .respond(returnSave);
              
              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/DataType/GetById'))
                  .respond(returnById);              
              
              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/DataType/GetEmpty'))
                  .respond(returnEmpty);
              
              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/DataType/GetPreValues'))
                  .respond(returnPreValues);
          },
          expectGetById: function() {
            $httpBackend
              .expectGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/DataType/GetById'));
          }
      };
  }]);

angular.module('umbraco.mocks').
  factory('entityMocks', ['$httpBackend', 'mocksUtils', function ($httpBackend, mocksUtils) {
      'use strict';

      function returnEntitybyId(status, data, headers) {

          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var id = mocksUtils.getParameterByName(data, "id") || "1234";
          id = parseInt(id, 10);

          var node = mocksUtils.getMockEntity(id);

          return [200, node, null];
      }

      function returnEntitybyIds(status, data, headers) {

          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var ids = mocksUtils.getParametersByName(data, "ids") || [1234, 23324, 2323, 23424];
          var nodes = [];

          $(ids).each(function (i, id) {
              var _id = parseInt(id, 10);
              nodes.push(mocksUtils.getMockEntity(_id));
          });

          return [200, nodes, null];
      }


      return {
          register: function () {

              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Entity/GetByIds'))
                  .respond(returnEntitybyIds);

              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Entity/GetAncestors'))
                  .respond(returnEntitybyIds);

              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Entity/GetById?'))
                  .respond(returnEntitybyId);
          }
      };
  }]);
angular.module('umbraco.mocks').
  factory('macroMocks', ['$httpBackend', 'mocksUtils', function ($httpBackend, mocksUtils) {
      'use strict';
      
      function returnParameters(status, data, headers) {

          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var nodes = [{
              alias: "parameter1",
              name: "Parameter 1"              
          }, {
              alias: "parameter2",
              name: "Parameter 2"
          }];
          
          return [200, nodes, null];
      }


      return {
          register: function () {

              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Macro/GetMacroParameters'))
                  .respond(returnParameters);

          }
      };
  }]);
angular.module('umbraco.mocks').
  factory('mediaMocks', ['$httpBackend', 'mocksUtils', function ($httpBackend, mocksUtils) {
      'use strict';
      
      function returnNodeCollection(status, data, headers){
        var nodes = [{"properties":[{"id":348,"value":"/media/1045/windows95.jpg","alias":"umbracoFile"},{"id":349,"value":"640","alias":"umbracoWidth"},{"id":350,"value":"472","alias":"umbracoHeight"},{"id":351,"value":"53472","alias":"umbracoBytes"},{"id":352,"value":"jpg","alias":"umbracoExtension"}],"updateDate":"2013-08-27 15:50:08","createDate":"2013-08-27 15:50:08","owner":{"id":0,"name":"admin"},"updator":null,"contentTypeAlias":"Image","sortOrder":0,"name":"windows95.jpg","id":1128,"icon":"mediaPhoto.gif","parentId":1127},{"properties":[{"id":353,"value":"/media/1046/pete.png","alias":"umbracoFile"},{"id":354,"value":"240","alias":"umbracoWidth"},{"id":355,"value":"240","alias":"umbracoHeight"},{"id":356,"value":"87408","alias":"umbracoBytes"},{"id":357,"value":"png","alias":"umbracoExtension"}],"updateDate":"2013-08-27 15:50:08","createDate":"2013-08-27 15:50:08","owner":{"id":0,"name":"admin"},"updator":null,"contentTypeAlias":"Image","sortOrder":1,"name":"pete.png","id":1129,"icon":"mediaPhoto.gif","parentId":1127},{"properties":[{"id":358,"value":"/media/1047/unicorn.jpg","alias":"umbracoFile"},{"id":359,"value":"640","alias":"umbracoWidth"},{"id":360,"value":"640","alias":"umbracoHeight"},{"id":361,"value":"577380","alias":"umbracoBytes"},{"id":362,"value":"jpg","alias":"umbracoExtension"}],"updateDate":"2013-08-27 15:50:09","createDate":"2013-08-27 15:50:09","owner":{"id":0,"name":"admin"},"updator":null,"contentTypeAlias":"Image","sortOrder":2,"name":"unicorn.jpg","id":1130,"icon":"mediaPhoto.gif","parentId":1127},{"properties":[{"id":363,"value":"/media/1049/exploding-head.gif","alias":"umbracoFile"},{"id":364,"value":"500","alias":"umbracoWidth"},{"id":365,"value":"279","alias":"umbracoHeight"},{"id":366,"value":"451237","alias":"umbracoBytes"},{"id":367,"value":"gif","alias":"umbracoExtension"}],"updateDate":"2013-08-27 15:50:09","createDate":"2013-08-27 15:50:09","owner":{"id":0,"name":"admin"},"updator":null,"contentTypeAlias":"Image","sortOrder":3,"name":"exploding head.gif","id":1131,"icon":"mediaPhoto.gif","parentId":1127},{"properties":[{"id":368,"value":"/media/1048/bighead.jpg","alias":"umbracoFile"},{"id":369,"value":"1240","alias":"umbracoWidth"},{"id":370,"value":"1655","alias":"umbracoHeight"},{"id":371,"value":"836261","alias":"umbracoBytes"},{"id":372,"value":"jpg","alias":"umbracoExtension"}],"updateDate":"2013-08-27 15:50:09","createDate":"2013-08-27 15:50:09","owner":{"id":0,"name":"admin"},"updator":null,"contentTypeAlias":"Image","sortOrder":4,"name":"bighead.jpg","id":1132,"icon":"mediaPhoto.gif","parentId":1127},{"properties":[{"id":373,"value":"/media/1050/powerlines.jpg","alias":"umbracoFile"},{"id":374,"value":"636","alias":"umbracoWidth"},{"id":375,"value":"423","alias":"umbracoHeight"},{"id":376,"value":"79874","alias":"umbracoBytes"},{"id":377,"value":"jpg","alias":"umbracoExtension"}],"updateDate":"2013-08-27 15:50:09","createDate":"2013-08-27 15:50:09","owner":{"id":0,"name":"admin"},"updator":null,"contentTypeAlias":"Image","sortOrder":5,"name":"powerlines.jpg","id":1133,"icon":"mediaPhoto.gif","parentId":1127},{"properties":[{"id":430,"value":"","alias":"contents"}],"updateDate":"2013-08-30 08:53:22","createDate":"2013-08-30 08:53:22","owner":{"id":0,"name":"admin"},"updator":null,"contentTypeAlias":"Folder","sortOrder":6,"name":"new folder","id":1146,"icon":"folder.gif","parentId":1127}];
        return [200, nodes, null];
      }

      function returnNodebyIds(status, data, headers) {
        var ids = mocksUtils.getParameterByName(data, "ids") || "1234,1234,4234";
        var items = [];
        
        _.each(ids, function(id){
          items.push(_getNode( parseInt( id, 10 )) );
        });

        return [200, items, null];
      }

      function returnNodebyId(status, data, headers) {

          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var id = mocksUtils.getParameterByName(data, "id") || 1234;
          id = parseInt(id, 10);


          
          return [200, _getNode(id), null];
      }
      
      function _getNode(id){
        var node = {
            name: "My media with id: " + id,
            updateDate: new Date(),
            publishDate: new Date(),
            id: id,
            parentId: 1234,
            icon: "icon-file-alt",
            owner: {name: "Administrator", id: 0},
            updater: {name: "Per Ploug Krogslund", id: 1},
            path: "-1,1234,2455", 
            tabs: [
            {
                label: "Media",
                alias: "tab0",
                id: 0,
                properties: [
                    { alias: "umbracoFile", label: "File", description:"Some file", view: "rte", value: "/media/1234/random.jpg" }
                ]
            }
            ]
        };

        return node;
      }

      return {
          register: function() {
            $httpBackend
              .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Media/GetById?'))
              .respond(returnNodebyId);

            $httpBackend
              .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Media/GetByIds?'))
              .respond(returnNodebyIds);
                            
            $httpBackend
              .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Media/GetChildren'))
              .respond(returnNodeCollection);

          },
          expectGetById: function() {
            $httpBackend
              .expectGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Media/GetById'));
          }
      };
  }]);

/**
* @ngdoc service
* @name umbraco.mocks.sectionMocks
* @description 
* Mocks data retrival for the sections
**/
function sectionMocks($httpBackend, mocksUtils) {

    /** internal method to mock the sections to be returned */
    function getSections() {
        
        if (!mocksUtils.checkAuth()) {
            return [401, null, null];
        }

        var sections = [
            { name: "Content", cssclass: "icon-umb-content", alias: "content" },
            { name: "Media", cssclass: "icon-umb-media", alias: "media" },
            { name: "Settings", cssclass: "icon-umb-settings", alias: "settings" },
            { name: "Developer", cssclass: "icon-umb-developer", alias: "developer" },
            { name: "Users", cssclass: "icon-umb-users", alias: "users" },
            { name: "Developer", cssclass: "icon-umb-developer", alias: "developer" },
            { name: "Users", cssclass: "icon-umb-users", alias: "users" }
        ];
        
        return [200, sections, null];
    }
    
    return {
        register: function () {
            $httpBackend
              .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Section/GetSections'))
              .respond(getSections);
        }
    };
}

angular.module('umbraco.mocks').factory('sectionMocks', ['$httpBackend', 'mocksUtils', sectionMocks]);

angular.module('umbraco.mocks').
  factory('treeMocks', ['$httpBackend', 'mocksUtils', function ($httpBackend, mocksUtils) {
      'use strict';
      
      function getMenuItems() {

          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var menu = [
              { name: "Create", cssclass: "plus", alias: "create", metaData: {} },

              { seperator: true, name: "Delete", cssclass: "remove", alias: "delete", metaData: {} },
              { name: "Move", cssclass: "move", alias: "move", metaData: {} },
              { name: "Copy", cssclass: "copy", alias: "copy", metaData: {} },
              { name: "Sort", cssclass: "sort", alias: "sort", metaData: {} },

              { seperator: true, name: "Publish", cssclass: "globe", alias: "publish", metaData: {} },
              { name: "Rollback", cssclass: "undo", alias: "rollback", metaData: {} },

              { seperator: true, name: "Permissions", cssclass: "lock", alias: "permissions", metaData: {} },
              { name: "Audit Trail", cssclass: "time", alias: "audittrail", metaData: {} },
              { name: "Notifications", cssclass: "envelope", alias: "notifications", metaData: {} },

              { seperator: true, name: "Hostnames", cssclass: "home", alias: "hostnames", metaData: {} },
              { name: "Public Access", cssclass: "group", alias: "publicaccess", metaData: {} },

              { seperator: true, name: "Reload", cssclass: "refresh", alias: "users", metaData: {} },
          
                { seperator: true, name: "Empty Recycle Bin", cssclass: "trash", alias: "emptyrecyclebin", metaData: {} }
          ];

          var result = {
              menuItems: menu,
              defaultAlias: "create"
          };

          return [200, result, null];
      }

      function returnChildren(status, data, headers) {
          
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var id = mocksUtils.getParameterByName(data, "id");
          var section = mocksUtils.getParameterByName(data, "treeType");
          var level = mocksUtils.getParameterByName(data, "level")+1;

          var url = "/umbraco/UmbracoTrees/ApplicationTreeApi/GetChildren?treeType=" + section + "&id=1234&level=" + level;
          var menuUrl = "/umbraco/UmbracoTrees/ApplicationTreeApi/GetMenu?treeType=" + section + "&id=1234&parentId=456";
          
          //hack to have create as default content action
          var action;
          if (section === "content") {
              action = "create";
          }

          var children = [
              { name: "child-of-" + section, childNodesUrl: url, id: level + "" + 1234, icon: "icon-document", children: [], expanded: false, hasChildren: true, level: level, menuUrl: menuUrl },
              { name: "random-name-" + section, childNodesUrl: url, id: level + "" + 1235, icon: "icon-document", children: [], expanded: false, hasChildren: true, level: level, menuUrl: menuUrl },
              { name: "random-name-" + section, childNodesUrl: url, id: level + "" + 1236, icon: "icon-document", children: [], expanded: false, hasChildren: true, level: level, menuUrl: menuUrl },
              { name: "random-name-" + section, childNodesUrl: url, id: level + "" + 1237, icon: "icon-document", routePath: "common/legacy/1237?p=" + encodeURI("developer/contentType.aspx?idequal1234"), children: [], expanded: false, hasChildren: true, level: level, menuUrl: menuUrl }
          ];

          return [200, children, null];
      }

      function returnDataTypes(status, data, headers) {
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }
          
          var children = [
              { name: "Textstring", childNodesUrl: null, id: 10, icon: "icon-document", children: [], expanded: false, hasChildren: false, level: 1,  menuUrl: null },
              { name: "Multiple textstring", childNodesUrl: null, id: 11, icon: "icon-document", children: [], expanded: false, hasChildren: false, level: 1,  menuUrl: null },
              { name: "Yes/No", childNodesUrl: null, id: 12, icon: "icon-document", children: [], expanded: false, hasChildren: false, level: 1,  menuUrl: null },
              { name: "Rich Text Editor", childNodesUrl: null, id: 13, icon: "icon-document", children: [], expanded: false, hasChildren: false, level: 1,  menuUrl: null }
          ];
          
          return [200, children, null];
      }
      
      function returnDataTypeMenu(status, data, headers) {
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var menu = [
              {
                   name: "Create", cssclass: "plus", alias: "create", metaData: {
                       jsAction: "umbracoMenuActions.CreateChildEntity"
                   }
              },              
              { seperator: true, name: "Reload", cssclass: "refresh", alias: "users", metaData: {} }
          ];

          return [200, menu, null];
      }

      function returnApplicationTrees(status, data, headers) {

          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }

          var section = mocksUtils.getParameterByName(data, "application");
          var url = "/umbraco/UmbracoTrees/ApplicationTreeApi/GetChildren?treeType=" + section + "&id=1234&level=1";
          var menuUrl = "/umbraco/UmbracoTrees/ApplicationTreeApi/GetMenu?treeType=" + section + "&id=1234&parentId=456";
          var t;
          switch (section) {

              case "content":
                  t = {
                      name: "content",
                      id: -1,
                      children: [
                          { name: "My website", id: 1234, childNodesUrl: url, icon: "icon-home", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl },
                          { name: "Components", id: 1235, childNodesUrl: url, icon: "icon-document", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl },
                          { name: "Archieve", id: 1236, childNodesUrl: url, icon: "icon-document", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl },
                          { name: "Recycle Bin", id: -20, childNodesUrl: url, icon: "icon-trash", routePath: section + "/recyclebin", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl }
                      ],
                      expanded: true,
                      hasChildren: true,
                      level: 0,
                      menuUrl: menuUrl,
                      metaData: { treeAlias: "content" }
                  };

                  break;
              case "media":
                  t = {
                      name: "media",
                      id: -1,
                      children: [
                          { name: "random-name-" + section, childNodesUrl: url, id: 1234, icon: "icon-home", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl },
                          { name: "random-name-" + section, childNodesUrl: url, id: 1235, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl },
                          { name: "random-name-" + section, childNodesUrl: url, id: 1236, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl },
                          { name: "random-name-" + section, childNodesUrl: url, id: 1237, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl }
                      ],
                      expanded: true,
                      hasChildren: true,
                      level: 0,
                      menuUrl: menuUrl,
                      metaData: { treeAlias: "media" }
                  };

                  break;
              case "developer":                  

                  var dataTypeChildrenUrl = "/umbraco/UmbracoTrees/DataTypeTree/GetNodes?id=-1&application=developer";
                  var dataTypeMenuUrl = "/umbraco/UmbracoTrees/DataTypeTree/GetMenu?id=-1&application=developer";

                  t = {
                      name: "developer",
                      id: -1,
                      children: [
                          { name: "Data types", childNodesUrl: dataTypeChildrenUrl, id: -1, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: dataTypeMenuUrl, metaData: { treeAlias: "datatype" } },
                          { name: "Macros", childNodesUrl: url, id: -1, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl, metaData: { treeAlias: "macros" } },
                          { name: "Packages", childNodesUrl: url, id: -1, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl, metaData: { treeAlias: "packager" } },
                          { name: "XSLT Files", childNodesUrl: url, id: -1, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl, metaData: { treeAlias: "xslt" } },
                          { name: "Partial View Macros", childNodesUrl: url, id: -1, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl, metaData: { treeAlias: "partialViewMacros" } }
                      ],
                      expanded: true,
                      hasChildren: true,
                      level: 0,
                      isContainer: true
                  };

                  break;
              case "settings":
                  t = {
                      name: "settings",
                      id: -1,
                      children: [
                          { name: "Stylesheets", childNodesUrl: url, id: -1, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl, metaData: { treeAlias: "stylesheets" } },
                          { name: "Templates", childNodesUrl: url, id: -1, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl, metaData: { treeAlias: "templates" } },
                          { name: "Dictionary", childNodesUrl: url, id: -1, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl, metaData: { treeAlias: "dictionary" } },
                          { name: "Media types", childNodesUrl: url, id: -1, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl, metaData: { treeAlias: "mediaTypes" } },
                          { name: "Document types", childNodesUrl: url, id: -1, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl, metaData: { treeAlias: "nodeTypes" } }
                      ],
                      expanded: true,
                      hasChildren: true,
                      level: 0,
                      isContainer: true
                  };
                  
                  break;
              default:
                  
                  t = {
                      name: "randomTree",
                      id: -1,
                      children: [
                          { name: "random-name-" + section, childNodesUrl: url, id: 1234, icon: "icon-home", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl },
                          { name: "random-name-" + section, childNodesUrl: url, id: 1235, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl },
                          { name: "random-name-" + section, childNodesUrl: url, id: 1236, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl },
                          { name: "random-name-" + section, childNodesUrl: url, id: 1237, icon: "icon-folder-close", children: [], expanded: false, hasChildren: true, level: 1, menuUrl: menuUrl }
                      ],
                      expanded: true,
                      hasChildren: true,
                      level: 0,
                      menuUrl: menuUrl,
                      metaData: { treeAlias: "randomTree" }
                  };

                  break;
          }

      
          return [200, t, null];
      }


      return {
          register: function() {
              
              $httpBackend
                 .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoTrees/ApplicationTreeApi/GetApplicationTrees'))
                 .respond(returnApplicationTrees);

              $httpBackend
                 .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoTrees/ApplicationTreeApi/GetChildren'))
                 .respond(returnChildren);
              

              $httpBackend
                 .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoTrees/DataTypeTree/GetNodes'))
                 .respond(returnDataTypes);
              
              $httpBackend
                 .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoTrees/DataTypeTree/GetMenu'))
                 .respond(returnDataTypeMenu);
              
              $httpBackend
                 .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoTrees/ApplicationTreeApi/GetMenu'))
                 .respond(getMenuItems);
              
          }
      };
  }]);
angular.module('umbraco.mocks').
  factory('userMocks', ['$httpBackend', 'mocksUtils', function ($httpBackend, mocksUtils) {
      'use strict';

      function generateMockedUser() {
          // Ensure a new user object each call
          return {
              name: "Per Ploug",
              email: "test@test.com",
              emailHash: "f9879d71855b5ff21e4963273a886bfc",
              id: 0,
              locale: 'da-DK',
              remainingAuthSeconds: 600
          };
      }

      function isAuthenticated() {
          //check for existence of a cookie so we can do login/logout in the belle app (ignore for tests).
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }
          else {
              return [200, null, null];
          }
      }

      function getCurrentUser(status, data, headers) {
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }
          else {
              return [200, generateMockedUser(), null];
          }
      }

      function getRemainingTimeoutSeconds(status, data, headers) {
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }
          else {
              return [200, 600, null];
          }
      }

      function returnUser(status, data, headers) {

          //set the cookie for loging
          mocksUtils.setAuth();

          return [200, generateMockedUser(), null];
      }
      
      function logout() {
          
          mocksUtils.clearAuth();

          return [200, null, null];

      }

      return {
          register: function() {
              
              $httpBackend
                  .whenPOST(mocksUtils.urlRegex('/umbraco/UmbracoApi/Authentication/PostLogin'))
                  .respond(returnUser);

              $httpBackend
                  .whenPOST(mocksUtils.urlRegex('/umbraco/UmbracoApi/Authentication/PostLogout'))
                  .respond(logout);

              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/UmbracoApi/Authentication/IsAuthenticated'))
                  .respond(isAuthenticated);

              $httpBackend
                  .whenGET('/umbraco/UmbracoApi/Authentication/GetCurrentUser')
                  .respond(getCurrentUser);

              $httpBackend
                  .whenGET('/umbraco/UmbracoApi/Authentication/GetRemainingTimeoutSeconds')
                  .respond(getRemainingTimeoutSeconds);

                
          }
      };
  }]);
angular.module('umbraco.mocks.services')
.factory('assetsService', function ($q) {

    return {
        loadCss : function(path, scope, attributes, timeout){
            var deferred = $q.defer();
            deferred.resolve();
            return deferred.promise;
        },
        loadJs : function(path, scope, attributes, timeout){
            var deferred = $q.defer();
            
            if(path[0] !== "/"){
                path = "/" + path;
            }   

            $.getScript( "base" + path, function( data, textStatus, jqxhr ) {
                deferred.resolve();
            });

            return deferred.promise;
        }
    };
});
angular.module('umbraco.mocks').
  factory('localizationMocks', ['$httpBackend', 'mocksUtils', function ($httpBackend, mocksUtils) {
      'use strict';
      
      function getLanguageResource(status, data, headers) {
          //check for existence of a cookie so we can do login/logout in the belle app (ignore for tests).
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }
          else {
              return [200, {
                  "actions_assignDomain": "Culture and Hostnames",
                  "actions_auditTrail": "Audit Trail",
                  "actions_browse": "Browse Node",
                  "actions_changeDocType": "Change Document Type",
                  "actions_copy": "Copy",
                  "actions_create": "Create",
                  "actions_createPackage": "Create Package",
                  "actions_delete": "Delete",
                  "actions_disable": "Disable",
                  "actions_emptyTrashcan": "Empty recycle bin",
                  "actions_exportDocumentType": "Export Document Type",
                  "actions_importDocumentType": "Import Document Type",
                  "actions_importPackage": "Import Package",
                  "actions_liveEdit": "Edit in Canvas",
                  "actions_logout": "Exit",
                  "actions_move": "Move",
                  "actions_notify": "Notifications",
                  "actions_protect": "Public access",
                  "actions_publish": "Publish",
                  "actions_unpublish": "Unpublish",
                  "actions_refreshNode": "Reload nodes",
                  "actions_republish": "Republish entire site",
                  "actions_rights": "Permissions",
                  "actions_rollback": "Rollback",
                  "actions_sendtopublish": "Send To Publish",
                  "actions_sendToTranslate": "Send To Translation",
                  "actions_sort": "Sort",
                  "actions_toPublish": "Send to publication",
                  "actions_translate": "Translate",
                  "actions_update": "Update",
                  "actions_exportContourForm": "Export form",
                  "actions_importContourForm": "Import form",
                  "actions_archiveContourForm": "Archive form",
                  "actions_unarchiveContourForm": "Unarchive form",
                  "actions_defaultValue": "Default value",
                  "assignDomain_permissionDenied": "Permission denied.",
                  "assignDomain_addNew": "Add new Domain",
                  "assignDomain_remove": "remove",
                  "assignDomain_invalidNode": "Invalid node.",
                  "assignDomain_invalidDomain": "Invalid domain format.",
                  "assignDomain_duplicateDomain": "Domain has already been assigned.",
                  "assignDomain_domain": "Domain",
                  "assignDomain_language": "Language",
                  "assignDomain_domainCreated": "New domain '%0%' has been created",
                  "assignDomain_domainDeleted": "Domain '%0%' is deleted",
                  "assignDomain_domainExists": "Domain '%0%' has already been assigned",
                  "assignDomain_domainHelp": "Valid domain names are: 'example.com', 'www.example.com', 'example.com:8080' or        'https://www.example.com/'.<br /><br />One-level paths in domains are supported, eg. 'example.com/en'. However, they        should be avoided. Better use the culture setting above.",
                  "assignDomain_domainUpdated": "Domain '%0%' has been updated",
                  "assignDomain_orEdit": "Edit Current Domains",
                  "assignDomain_inherit": "Inherit",
                  "assignDomain_setLanguage": "Culture",
                  "assignDomain_setLanguageHelp": "Set the culture for nodes below the current node,<br /> or inherit culture from parent nodes. Will also apply<br />      to the current node, unless a domain below applies too.",
                  "assignDomain_setDomains": "Domains",
                  "auditTrails_atViewingFor": "Viewing for",
                  "buttons_select": "Select",
                  "buttons_somethingElse": "Do something else",
                  "buttons_bold": "Bold",
                  "buttons_deindent": "Cancel Paragraph Indent",
                  "buttons_formFieldInsert": "Insert form field",
                  "buttons_graphicHeadline": "Insert graphic headline",
                  "buttons_htmlEdit": "Edit Html",
                  "buttons_indent": "Indent Paragraph",
                  "buttons_italic": "Italic",
                  "buttons_justifyCenter": "Center",
                  "buttons_justifyLeft": "Justify Left",
                  "buttons_justifyRight": "Justify Right",
                  "buttons_linkInsert": "Insert Link",
                  "buttons_linkLocal": "Insert local link (anchor)",
                  "buttons_listBullet": "Bullet List",
                  "buttons_listNumeric": "Numeric List",
                  "buttons_macroInsert": "Insert macro",
                  "buttons_pictureInsert": "Insert picture",
                  "buttons_relations": "Edit relations",
                  "buttons_save": "Save",
                  "buttons_saveAndPublish": "Save and publish",
                  "buttons_saveToPublish": "Save and send for approval",
                  "buttons_showPage": "Preview",
                  "buttons_showPageDisabled": "Preview is disabled because there's no template assigned",
                  "buttons_styleChoose": "Choose style",
                  "buttons_styleShow": "Show styles",
                  "buttons_tableInsert": "Insert table",
                  "changeDocType_changeDocTypeInstruction": "To change the document type for the selected content, first select from the list of valid types for this location.",
                  "changeDocType_changeDocTypeInstruction2": "Then confirm and/or amend the mapping of properties from the current type to the new, and click Save.",
                  "changeDocType_contentRepublished": "The content has been re-published.",
                  "changeDocType_currentProperty": "Current Property",
                  "changeDocType_currentType": "Current type",
                  "changeDocType_docTypeCannotBeChanged": "The document type cannot be changed, as there are no alternatives valid for this location.",
                  "changeDocType_docTypeChanged": "Document Type Changed",
                  "changeDocType_mapProperties": "Map Properties",
                  "changeDocType_mapToProperty": "Map to Property",
                  "changeDocType_newTemplate": "New Template",
                  "changeDocType_newType": "New Type",
                  "changeDocType_none": "none",
                  "changeDocType_selectedContent": "Content",
                  "changeDocType_selectNewDocType": "Select New Document Type",
                  "changeDocType_successMessage": "The document type of the selected content has been successfully changed to [new type] and the following properties mapped:",
                  "changeDocType_to": "to",
                  "changeDocType_validationErrorPropertyWithMoreThanOneMapping": "Could not complete property mapping as one or more properties have more than one mapping defined.",
                  "changeDocType_validDocTypesNote": "Only alternate types valid for the current location are displayed.",
                  "content_about": "About this page",
                  "content_alias": "Alias",
                  "content_alternativeTextHelp": "(how would you describe the picture over the phone)",
                  "content_alternativeUrls": "Alternative Links",
                  "content_clickToEdit": "Click to edit this item",
                  "content_createBy": "Created by",
                  "content_createByDesc": "Original autho",
                  "content_updatedBy": "Updated by",
                  "content_createDate": "Created",
                  "content_createDateDesc": "Date/time this document was created",
                  "content_documentType": "Document Type",
                  "content_editing": "Editing",
                  "content_expireDate": "Remove at",
                  "content_itemChanged": "This item has been changed after publication",
                  "content_itemNotPublished": "This item is not published",
                  "content_lastPublished": "Last published",
                  "content_listViewNoItems": "There are no items show in the list.",
                  "content_mediatype": "Media Type",
                  "content_mediaLinks": "Link to media item(s)",
                  "content_membergroup": "Member Group",
                  "content_memberrole": "Role",
                  "content_membertype": "Member Type",
                  "content_noDate": "No date chosen",
                  "content_nodeName": "Page Title",
                  "content_otherElements": "Properties",
                  "content_parentNotPublished": "This document is published but is not visible because the parent '%0%' is unpublished",
                  "content_parentNotPublishedAnomaly": "Oops: this document is published but is not in the cache (internal error)",
                  "content_publish": "Publish",
                  "content_publishStatus": "Publication Status",
                  "content_releaseDate": "Publish at",
                  "content_removeDate": "Clear Date",
                  "content_sortDone": "Sortorder is updated",
                  "content_sortHelp": "To sort the nodes, simply drag the nodes or click one of the column headers. You can select multiple nodes by holding the 'shift' or 'control' key while selecting",
                  "content_statistics": "Statistics",
                  "content_titleOptional": "Title (optional)",
                  "content_type": "Type",
                  "content_unPublish": "Unpublish",
                  "content_updateDate": "Last edited",
                  "content_updateDateDesc": "Date/time this document was created",
                  "content_uploadClear": "Remove file",
                  "content_urls": "Link to document",
                  "content_memberof": "Member of group(s)",
                  "content_notmemberof": "Not a member of group(s)",
                  "content_childItems": "Child items",
                  "create_chooseNode": "Where do you want to create the new %0%",
                  "create_createUnder": "Create a page under",
                  "create_updateData": "Choose a type and a title",
                  "create_noDocumentTypes": "There are no allowed document types available. You must enable these in the settings section under <strong>'document types'</strong>.",
                  "create_noMediaTypes": "There are no allowed media types available. You must enable these in the settings section under <strong>'media types'</strong>.",
                  "dashboard_browser": "Browse your website",
                  "dashboard_dontShowAgain": "- Hide",
                  "dashboard_nothinghappens": "If umbraco isn't opening, you might need to allow popups from this site",
                  "dashboard_openinnew": "has opened in a new window",
                  "dashboard_restart": "Restart",
                  "dashboard_visit": "Visit",
                  "dashboard_welcome": "Welcome",
                  "defaultdialogs_anchorInsert": "Name",
                  "defaultdialogs_assignDomain": "Manage hostnames",
                  "defaultdialogs_closeThisWindow": "Close this window",
                  "defaultdialogs_confirmdelete": "Are you sure you want to delete",
                  "defaultdialogs_confirmdisable": "Are you sure you want to disable",
                  "defaultdialogs_confirmEmptyTrashcan": "Please check this box to confirm deletion of %0% item(s)",
                  "defaultdialogs_confirmlogout": "Are you sure?",
                  "defaultdialogs_confirmSure": "Are you sure?",
                  "defaultdialogs_cut": "Cut",
                  "defaultdialogs_editdictionary": "Edit Dictionary Item",
                  "defaultdialogs_editlanguage": "Edit Language",
                  "defaultdialogs_insertAnchor": "Insert local link",
                  "defaultdialogs_insertCharacter": "Insert character",
                  "defaultdialogs_insertgraphicheadline": "Insert graphic headline",
                  "defaultdialogs_insertimage": "Insert picture",
                  "defaultdialogs_insertlink": "Insert link",
                  "defaultdialogs_insertMacro": "Click to add a Macro",
                  "defaultdialogs_inserttable": "Insert table",
                  "defaultdialogs_lastEdited": "Last Edited",
                  "defaultdialogs_link": "Link",
                  "defaultdialogs_linkinternal": "Internal link:",
                  "defaultdialogs_linklocaltip": "When using local links, insert '#' infront of link",
                  "defaultdialogs_linknewwindow": "Open in new window?",
                  "defaultdialogs_macroContainerSettings": "Macro Settings",
                  "defaultdialogs_macroDoesNotHaveProperties": "This macro does not contain any properties you can edit",
                  "defaultdialogs_paste": "Paste",
                  "defaultdialogs_permissionsEdit": "Edit Permissions for",
                  "defaultdialogs_recycleBinDeleting": "The items in the recycle bin are now being deleted. Please do not close this window while this operation takes place",
                  "defaultdialogs_recycleBinIsEmpty": "The recycle bin is now empty",
                  "defaultdialogs_recycleBinWarning": "When items are deleted from the recycle bin, they will be gone forever",
                  "defaultdialogs_regexSearchError": "<a target='_blank' href='http://regexlib.com'>regexlib.com</a>'s webservice is currently experiencing some problems, which we have no control over. We are very sorry for this inconvenience.",
                  "defaultdialogs_regexSearchHelp": "Search for a regular expression to add validation to a form field. Exemple: 'email, 'zip-code' 'url'",
                  "defaultdialogs_removeMacro": "Remove Macro",
                  "defaultdialogs_requiredField": "Required Field",
                  "defaultdialogs_sitereindexed": "Site is reindexed",
                  "defaultdialogs_siterepublished": "The website cache has been refreshed. All publish content is now uptodate. While all unpublished content is still unpublished",
                  "defaultdialogs_siterepublishHelp": "The website cache will be refreshed. All published content will be updated, while unpublished content will stay unpublished.",
                  "defaultdialogs_tableColumns": "Number of columns",
                  "defaultdialogs_tableRows": "Number of rows",
                  "defaultdialogs_templateContentAreaHelp": "<strong>Set a placeholder id</strong> by setting an ID on your placeholder you can inject content into this template from child templates,      by refering this ID using a <code>&lt;asp:content /&gt;</code> element.",
                  "defaultdialogs_templateContentPlaceHolderHelp": "<strong>Select a placeholder id</strong> from the list below. You can only      choose Id's from the current template's master.",
                  "defaultdialogs_thumbnailimageclickfororiginal": "Click on the image to see full size",
                  "defaultdialogs_treepicker": "Pick item",
                  "defaultdialogs_viewCacheItem": "View Cache Item",
                  "dictionaryItem_description": "Edit the different language versions for the dictionary item '<em>%0%</em>' below<br/>You can add additional languages under the 'languages' in the menu on the left   ",
                  "dictionaryItem_displayName": "Culture Name",
                  "placeholders_username": "Enter your username",
                  "placeholders_password": "Enter your password",
                  "placeholders_entername": "Enter a name...",
                  "placeholders_nameentity": "Name the %0%...",
                  "placeholders_search": "Type to search...",
                  "placeholders_filter": "Type to filter...",
                  "editcontenttype_allowedchildnodetypes": "Allowed child nodetypes",
                  "editcontenttype_create": "Create",
                  "editcontenttype_deletetab": "Delete tab",
                  "editcontenttype_description": "Description",
                  "editcontenttype_newtab": "New tab",
                  "editcontenttype_tab": "Tab",
                  "editcontenttype_thumbnail": "Thumbnail",
                  "editcontenttype_iscontainercontenttype": "Use as container content type",
                  "editdatatype_addPrevalue": "Add prevalue",
                  "editdatatype_dataBaseDatatype": "Database datatype",
                  "editdatatype_guid": "Property editor GUID",
                  "editdatatype_renderControl": "Property editor",
                  "editdatatype_rteButtons": "Buttons",
                  "editdatatype_rteEnableAdvancedSettings": "Enable advanced settings for",
                  "editdatatype_rteEnableContextMenu": "Enable context menu",
                  "editdatatype_rteMaximumDefaultImgSize": "Maximum default size of inserted images",
                  "editdatatype_rteRelatedStylesheets": "Related stylesheets",
                  "editdatatype_rteShowLabel": "Show label",
                  "editdatatype_rteWidthAndHeight": "Width and height",
                  "errorHandling_errorButDataWasSaved": "Your data has been saved, but before you can publish this page there are some errors you need to fix first:",
                  "errorHandling_errorChangingProviderPassword": "The current MemberShip Provider does not support changing password (EnablePasswordRetrieval need to be true)",
                  "errorHandling_errorExistsWithoutTab": "%0% already exists",
                  "errorHandling_errorHeader": "There were errors:",
                  "errorHandling_errorHeaderWithoutTab": "There were errors:",
                  "errorHandling_errorInPasswordFormat": "The password should be a minimum of %0% characters long and contain at least %1% non-alpha numeric character(s)",
                  "errorHandling_errorIntegerWithoutTab": "%0% must be an integer",
                  "errorHandling_errorMandatory": "The %0% field in the %1% tab is mandatory",
                  "errorHandling_errorMandatoryWithoutTab": "%0% is a mandatory field",
                  "errorHandling_errorRegExp": "%0% at %1% is not in a correct format",
                  "errorHandling_errorRegExpWithoutTab": "%0% is not in a correct format",
                  "errors_dissallowedMediaType": "The specified file type has been dissallowed by the administrator",
                  "errors_codemirroriewarning": "NOTE! Even though CodeMirror is enabled by configuration, it is disabled in Internet Explorer because it's not stable enough.",
                  "errors_contentTypeAliasAndNameNotNull": "Please fill both alias and name on the new propertytype!",
                  "errors_filePermissionsError": "There is a problem with read/write access to a specific file or folder",
                  "errors_missingTitle": "Please enter a title",
                  "errors_missingType": "Please choose a type",
                  "errors_pictureResizeBiggerThanOrg": "You're about to make the picture larger than the original size. Are you sure that you want to proceed?",
                  "errors_pythonErrorHeader": "Error in python script",
                  "errors_pythonErrorText": "The python script has not been saved, because it contained error(s)",
                  "errors_startNodeDoesNotExists": "Startnode deleted, please contact your administrator",
                  "errors_stylesMustMarkBeforeSelect": "Please mark content before changing style",
                  "errors_stylesNoStylesOnPage": "No active styles available",
                  "errors_tableColMergeLeft": "Please place cursor at the left of the two cells you wish to merge",
                  "errors_tableSplitNotSplittable": "You cannot split a cell that hasn't been merged.",
                  "errors_xsltErrorHeader": "Error in XSLT source",
                  "errors_xsltErrorText": "The XSLT has not been saved, because it contained error(s)",
                  "general_about": "About",
                  "general_action": "Action",
                  "general_add": "Add",
                  "general_alias": "Alias",
                  "general_areyousure": "Are you sure?",
                  "general_border": "Border",
                  "general_by": "or",
                  "general_cancel": "Cancel",
                  "general_cellMargin": "Cell margin",
                  "general_choose": "Choose",
                  "general_close": "Close",
                  "general_closewindow": "Close Window",
                  "general_comment": "Comment",
                  "general_confirm": "Confirm",
                  "general_constrainProportions": "Constrain proportions",
                  "general_continue": "Continue",
                  "general_copy": "Copy",
                  "general_create": "Create",
                  "general_database": "Database",
                  "general_date": "Date",
                  "general_default": "Default",
                  "general_delete": "Delete",
                  "general_deleted": "Deleted",
                  "general_deleting": "Deleting...",
                  "general_design": "Design",
                  "general_dimensions": "Dimensions",
                  "general_down": "Down",
                  "general_download": "Download",
                  "general_edit": "Edit",
                  "general_edited": "Edited",
                  "general_elements": "Elements",
                  "general_email": "Email",
                  "general_error": "Error",
                  "general_findDocument": "Find",
                  "general_height": "Height",
                  "general_help": "Help",
                  "general_icon": "Icon",
                  "general_import": "Import",
                  "general_innerMargin": "Inner margin",
                  "general_insert": "Insert",
                  "general_install": "Install",
                  "general_justify": "Justify",
                  "general_language": "Language",
                  "general_layout": "Layout",
                  "general_loading": "Loading",
                  "general_locked": "Locked",
                  "general_login": "Login",
                  "general_logoff": "Log off",
                  "general_logout": "Logout",
                  "general_macro": "Macro",
                  "general_move": "Move",
                  "general_name": "Name",
                  "general_new": "New",
                  "general_next": "Next",
                  "general_no": "No",
                  "general_of": "of",
                  "general_ok": "OK",
                  "general_open": "Open",
                  "general_or": "or",
                  "general_password": "Password",
                  "general_path": "Path",
                  "general_placeHolderID": "Placeholder ID",
                  "general_pleasewait": "One moment please...",
                  "general_previous": "Previous",
                  "general_properties": "Properties",
                  "general_reciept": "Email to receive form data",
                  "general_recycleBin": "Recycle Bin",
                  "general_remaining": "Remaining",
                  "general_rename": "Rename",
                  "general_renew": "Renew",
                  "general_required": "Required",
                  "general_retry": "Retry",
                  "general_rights": "Permissions",
                  "general_search": "Search",
                  "general_server": "Server",
                  "general_show": "Show",
                  "general_showPageOnSend": "Show page on Send",
                  "general_size": "Size",
                  "general_sort": "Sort",
                  "general_type": "Type",
                  "general_typeToSearch": "Type to search...",
                  "general_up": "Up",
                  "general_update": "Update",
                  "general_upgrade": "Upgrade",
                  "general_upload": "Upload",
                  "general_url": "Url",
                  "general_user": "User",
                  "general_username": "Username",
                  "general_value": "Value",
                  "general_view": "View",
                  "general_welcome": "Welcome...",
                  "general_width": "Width",
                  "general_yes": "Yes",
                  "general_folder": "Folder",
                  "general_searchResults": "Search results",
                  "graphicheadline_backgroundcolor": "Background color",
                  "graphicheadline_bold": "Bold",
                  "graphicheadline_color": "Text color",
                  "graphicheadline_font": "Font",
                  "graphicheadline_text": "Text",
                  "headers_page": "Page",
                  "installer_databaseErrorCannotConnect": "The installer cannot connect to the database.",
                  "installer_databaseErrorWebConfig": "Could not save the web.config file. Please modify the connection string manually.",
                  "installer_databaseFound": "Your database has been found and is identified as",
                  "installer_databaseHeader": "Database configuration",
                  "installer_databaseInstall": "      Press the <strong>install</strong> button to install the Umbraco %0% database    ",
                  "installer_databaseInstallDone": "Umbraco %0% has now been copied to your database. Press <strong>Next</strong> to proceed.",
                  "installer_databaseNotFound": "<p>Database not found! Please check that the information in the 'connection string' of the \"web.config\" file is correct.</p>              <p>To proceed, please edit the 'web.config' file (using Visual Studio or your favourite text editor), scroll to the bottom, add the connection string for your database in the key named 'umbracoDbDSN' and save the file. </p>              <p>              Click the <strong>retry</strong> button when               done.<br /><a href='http://umbraco.org/redir/installWebConfig' target='_blank'>              More information on editing web.config here.</a></p>",
                  "installer_databaseText": "To complete this step, you must know some information regarding your database server ('connection string').<br />        Please contact your ISP if necessary.        If you're installing on a local machine or server you might need information from your system administrator.",
                  "installer_databaseUpgrade": "      <p>      Press the <strong>upgrade</strong> button to upgrade your database to Umbraco %0%</p>      <p>      Don't worry - no content will be deleted and everything will continue working afterwards!      </p>          ",
                  "installer_databaseUpgradeDone": "Your database has been upgraded to the final version %0%.<br />Press <strong>Next</strong> to       proceed. ",
                  "installer_databaseUpToDate": "Your current database is up-to-date!. Click <strong>next</strong> to continue the configuration wizard",
                  "installer_defaultUserChangePass": "<strong>The Default users' password needs to be changed!</strong>",
                  "installer_defaultUserDisabled": "<strong>The Default user has been disabled or has no access to umbraco!</strong></p><p>No further actions needs to be taken. Click <b>Next</b> to proceed.",
                  "installer_defaultUserPassChanged": "<strong>The Default user's password has been successfully changed since the installation!</strong></p><p>No further actions needs to be taken. Click <strong>Next</strong> to proceed.",
                  "installer_defaultUserPasswordChanged": "The password is changed!",
                  "installer_defaultUserText": "        <p>          umbraco creates a default user with a login <strong>('admin')</strong> and password <strong>('default')</strong>. It's <strong>important</strong> that the password is           changed to something unique.        </p>        <p>          This step will check the default user's password and suggest if it needs to be changed.        </p>      ",
                  "installer_greatStart": "Get a great start, watch our introduction videos",
                  "installer_licenseText": "By clicking the next button (or modifying the umbracoConfigurationStatus in web.config), you accept the license for this software as specified in the box below. Notice that this umbraco distribution consists of two different licenses, the open source MIT license for the framework and the umbraco freeware license that covers the UI.",
                  "installer_None": "Not installed yet.",
                  "installer_permissionsAffectedFolders": "Affected files and folders",
                  "installer_permissionsAffectedFoldersMoreInfo": "More information on setting up permissions for umbraco here",
                  "installer_permissionsAffectedFoldersText": "You need to grant ASP.NET modify permissions to the following files/folders",
                  "installer_permissionsAlmostPerfect": "<strong>Your permission settings are almost perfect!</strong><br /><br />        You can run umbraco without problems, but you will not be able to install packages which are recommended to take full advantage of umbraco.",
                  "installer_permissionsHowtoResolve": "How to Resolve",
                  "installer_permissionsHowtoResolveLink": "Click here to read the text version",
                  "installer_permissionsHowtoResolveText": "Watch our <strong>video tutorial</strong> on setting up folder permissions for umbraco or read the text version.",
                  "installer_permissionsMaybeAnIssue": "<strong>Your permission settings might be an issue!</strong>      <br/><br />      You can run umbraco without problems, but you will not be able to create folders or install packages which are recommended to take full advantage of umbraco.",
                  "installer_permissionsNotReady": "<strong>Your permission settings are not ready for umbraco!</strong>          <br /><br />          In order to run umbraco, you'll need to update your permission settings.",
                  "installer_permissionsPerfect": "<strong>Your permission settings are perfect!</strong><br /><br />              You are ready to run umbraco and install packages!",
                  "installer_permissionsResolveFolderIssues": "Resolving folder issue",
                  "installer_permissionsResolveFolderIssuesLink": "Follow this link for more information on problems with ASP.NET and creating folders",
                  "installer_permissionsSettingUpPermissions": "Setting up folder permissions",
                  "installer_permissionsText": "      umbraco needs write/modify access to certain directories in order to store files like pictures and PDF's.      It also stores temporary data (aka: cache) for enhancing the performance of your website.    ",
                  "installer_runwayFromScratch": "I want to start from scratch",
                  "installer_runwayFromScratchText": "        Your website is completely empty at the moment, so that's perfect if you want to start from scratch and create your own document types and templates.         (<a href='http://umbraco.tv/documentation/videos/for-site-builders/foundation/document-types'>learn how</a>)        You can still choose to install Runway later on. Please go to the Developer section and choose Packages.      ",
                  "installer_runwayHeader": "You've just set up a clean Umbraco platform. What do you want to do next?",
                  "installer_runwayInstalled": "Runway is installed",
                  "installer_runwayInstalledText": "      You have the foundation in place. Select what modules you wish to install on top of it.<br />      This is our list of recommended modules, check off the ones you would like to install, or view the <a href='#' onclick='toggleModules(); return false;' id='toggleModuleList'>full list of modules</a>      ",
                  "installer_runwayOnlyProUsers": "Only recommended for experienced users",
                  "installer_runwaySimpleSite": "I want to start with a simple website",
                  "installer_runwaySimpleSiteText": "      <p>      'Runway' is a simple website providing some basic document types and templates. The installer can set up Runway for you automatically,         but you can easily edit, extend or remove it. It's not necessary and you can perfectly use Umbraco without it. However,         Runway offers an easy foundation based on best practices to get you started faster than ever.        If you choose to install Runway, you can optionally select basic building blocks called Runway Modules to enhance your Runway pages.        </p>        <small>        <em>Included with Runway:</em> Home page, Getting Started page, Installing Modules page.<br />        <em>Optional Modules:</em> Top Navigation, Sitemap, Contact, Gallery.        </small>      ",
                  "installer_runwayWhatIsRunway": "What is Runway",
                  "installer_step1": "Step 1/5 Accept license",
                  "installer_step2": "Step 2/5: Database configuration",
                  "installer_step3": "Step 3/5: Validating File Permissions",
                  "installer_step4": "Step 4/5: Check umbraco security",
                  "installer_step5": "Step 5/5: Umbraco is ready to get you started",
                  "installer_thankYou": "Thank you for choosing umbraco",
                  "installer_theEndBrowseSite": "<h3>Browse your new site</h3>You installed Runway, so why not see how your new website looks.",
                  "installer_theEndFurtherHelp": "<h3>Further help and information</h3>Get help from our award winning community, browse the documentation or watch some free videos on how to build a simple site, how to use packages and a quick guide to the umbraco terminology",
                  "installer_theEndHeader": "Umbraco %0% is installed and ready for use",
                  "installer_theEndInstallFailed": "To finish the installation, you'll need to         manually edit the <strong>/web.config file</strong> and update the AppSetting key <strong>umbracoConfigurationStatus</strong> in the bottom to the value of <strong>'%0%'</strong>.",
                  "installer_theEndInstallSuccess": "You can get <strong>started instantly</strong> by clicking the 'Launch Umbraco' button below. <br />If you are <strong>new to umbraco</strong>, you can find plenty of resources on our getting started pages.",
                  "installer_theEndOpenUmbraco": "<h3>Launch Umbraco</h3>To manage your website, simply open the umbraco back office and start adding content, updating the templates and stylesheets or add new functionality",
                  "installer_Unavailable": "Connection to database failed.",
                  "installer_Version3": "Umbraco Version 3",
                  "installer_Version4": "Umbraco Version 4",
                  "installer_watch": "Watch",
                  "installer_welcomeIntro": "This wizard will guide you through the process of configuring <strong>umbraco %0%</strong> for a fresh install or upgrading from version 3.0.                                <br /><br />                                Press <strong>'next'</strong> to start the wizard.",
                  "language_cultureCode": "Culture Code",
                  "language_displayName": "Culture Name",
                  "lockout_lockoutWillOccur": "You've been idle and logout will automatically occur in",
                  "lockout_renewSession": "Renew now to save your work",
                  "login_greeting1": "Happy super Sunday",
                  "login_greeting2": "Happy manic Monday ",
                  "login_greeting3": "Happy tremendous Tuesday",
                  "login_greeting4": "Happy wonderful Wednesday",
                  "login_greeting5": "Happy thunderous Thursday",
                  "login_greeting6": "Happy friendly Friday",
                  "login_greeting7": "Happy shiny Saturday",
                  "login_instruction": "Log in below:",
                  "login_bottomText": "<p style='text-align:right;'>&copy; 2001 - %0% <br /><a href='http://umbraco.org' style='text-decoration: none' target='_blank'>umbraco.org</a></p> ",
                  "main_dashboard": "Dashboard",
                  "main_sections": "Sections",
                  "main_tree": "Content",
                  "moveOrCopy_choose": "Choose page above...",
                  "moveOrCopy_copyDone": "%0% has been copied to %1%",
                  "moveOrCopy_copyTo": "Select where the document %0% should be copied to below",
                  "moveOrCopy_moveDone": "%0% has been moved to %1%",
                  "moveOrCopy_moveTo": "Select where the document %0% should be moved to below",
                  "moveOrCopy_nodeSelected": "has been selected as the root of your new content, click 'ok' below.",
                  "moveOrCopy_noNodeSelected": "No node selected yet, please select a node in the list above before clicking 'ok'",
                  "moveOrCopy_notAllowedByContentType": "The current node is not allowed under the chosen node because of its type",
                  "moveOrCopy_notAllowedByPath": "The current node cannot be moved to one of its subpages",
                  "moveOrCopy_notAllowedAtRoot": "The current node cannot exist at the root",
                  "moveOrCopy_notValid": "The action isn't allowed since you have insufficient permissions on 1 or more child documents.",
                  "moveOrCopy_relateToOriginal": "Relate copied items to original",
                  "notifications_editNotifications": "Edit your notification for %0%",
                  "notifications_mailBody": "      Hi %0%      This is an automated mail to inform you that the task '%1%'      has been performed on the page '%2%'      by the user '%3%'      Go to http://%4%/actions/editContent.aspx?id=%5% to edit.      Have a nice day!      Cheers from the umbraco robot    ",
                  "notifications_mailBodyHtml": "<p>Hi %0%</p>  <p>This is an automated mail to inform you that the task <strong>'%1%'</strong>   has been performed on the page <a href='http://%4%/actions/preview.aspx?id=%5%'><strong>'%2%'</strong></a>  by the user <strong>'%3%'</strong>  </p>  <div style='margin: 8px 0; padding: 8px; display: block;'><br /><a style='color: white; font-weight: bold; background-color: #66cc66; text-decoration : none; margin-right: 20px; border: 8px solid #66cc66; width: 150px;' href='http://%4%/actions/publish.aspx?id=%5%'>&nbsp;&nbsp;PUBLISH&nbsp;&nbsp;</a> &nbsp; <a style='color: white; font-weight: bold; background-color: #5372c3; text-decoration : none; margin-right: 20px; border: 8px solid #5372c3; width: 150px;' href='http://%4%/actions/editContent.aspx?id=%5%'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;EDIT&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a> &nbsp; <a style='color: white; font-weight: bold; background-color: #ca4a4a; text-decoration : none; margin-right: 20px; border: 8px solid #ca4a4a; width: 150px;' href='http://%4%/actions/delete.aspx?id=%5%'>&nbsp;&nbsp;&nbsp;&nbsp;DELETE&nbsp;&nbsp;&nbsp;&nbsp;</a><br />  </div>  <p>  <h3>Update summary:</h3>  <table style='width: 100%;'>   %6%</table> </p>  <div style='margin: 8px 0; padding: 8px; display: block;'><br /><a style='color: white; font-weight: bold; background-color: #66cc66; text-decoration : none; margin-right: 20px; border: 8px solid #66cc66; width: 150px;' href='http://%4%/actions/publish.aspx?id=%5%'>&nbsp;&nbsp;PUBLISH&nbsp;&nbsp;</a> &nbsp; <a style='color: white; font-weight: bold; background-color: #5372c3; text-decoration : none; margin-right: 20px; border: 8px solid #5372c3; width: 150px;' href='http://%4%/actions/editContent.aspx?id=%5%'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;EDIT&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a> &nbsp; <a style='color: white; font-weight: bold; background-color: #ca4a4a; text-decoration : none; margin-right: 20px; border: 8px solid #ca4a4a; width: 150px;' href='http://%4%/actions/delete.aspx?id=%5%'>&nbsp;&nbsp;&nbsp;&nbsp;DELETE&nbsp;&nbsp;&nbsp;&nbsp;</a><br />  </div>  <p>Have a nice day!<br /><br />  Cheers from the umbraco robot  </p>",
                  "notifications_mailSubject": "[%0%] Notification about %1% performed on %2%",
                  "notifications_notifications": "Notifications",
                  "packager_chooseLocalPackageText": "      Choose Package from your machine, by clicking the Browse<br />         button and locating the package. umbraco packages usually have a '.umb' or '.zip' extension.      ",
                  "packager_packageAuthor": "Author",
                  "packager_packageDemonstration": "Demonstration",
                  "packager_packageDocumentation": "Documentation",
                  "packager_packageMetaData": "Package meta data",
                  "packager_packageName": "Package name",
                  "packager_packageNoItemsHeader": "Package doesn't contain any items",
                  "packager_packageNoItemsText": "This package file doesn't contain any items to uninstall.<br/><br/>      You can safely remove this from the system by clicking 'uninstall package' below.",
                  "packager_packageNoUpgrades": "No upgrades available",
                  "packager_packageOptions": "Package options",
                  "packager_packageReadme": "Package readme",
                  "packager_packageRepository": "Package repository",
                  "packager_packageUninstallConfirm": "Confirm uninstall",
                  "packager_packageUninstalledHeader": "Package was uninstalled",
                  "packager_packageUninstalledText": "The package was successfully uninstalled",
                  "packager_packageUninstallHeader": "Uninstall package",
                  "packager_packageUninstallText": "You can unselect items you do not wish to remove, at this time, below. When you click 'confirm uninstall' all checked-off items will be removed.<br />      <span style='color: Red; font-weight: bold;'>Notice:</span> any documents, media etc depending on the items you remove, will stop working, and could lead to system instability,      so uninstall with caution. If in doubt, contact the package author.",
                  "packager_packageUpgradeDownload": "Download update from the repository",
                  "packager_packageUpgradeHeader": "Upgrade package",
                  "packager_packageUpgradeInstructions": "Upgrade instructions",
                  "packager_packageUpgradeText": " There's an upgrade available for this package. You can download it directly from the umbraco package repository.",
                  "packager_packageVersion": "Package version",
                  "packager_packageVersionHistory": "Package version history",
                  "packager_viewPackageWebsite": "View package website",
                  "paste_doNothing": "Paste with full formatting (Not recommended)",
                  "paste_errorMessage": "The text you're trying to paste contains special characters or formatting. This could be caused by copying text from Microsoft Word. umbraco can remove special characters or formatting automatically, so the pasted content will be more suitable for the web.",
                  "paste_removeAll": "Paste as raw text without any formatting at all",
                  "paste_removeSpecialFormattering": "Paste, but remove formatting (Recommended)",
                  "publicAccess_paAdvanced": "Role based protection",
                  "publicAccess_paAdvancedHelp": "If you wish to control access to the page using role-based authentication,<br /> using umbraco's member groups.",
                  "publicAccess_paAdvancedNoGroups": "You need to create a membergroup before you can use <br />role-based authentication.",
                  "publicAccess_paErrorPage": "Error Page",
                  "publicAccess_paErrorPageHelp": "Used when people are logged on, but do not have access",
                  "publicAccess_paHowWould": "Choose how to restict access to this page",
                  "publicAccess_paIsProtected": "%0% is now protected",
                  "publicAccess_paIsRemoved": "Protection removed from %0%",
                  "publicAccess_paLoginPage": "Login Page",
                  "publicAccess_paLoginPageHelp": "Choose the page that has the login formular",
                  "publicAccess_paRemoveProtection": "Remove Protection",
                  "publicAccess_paSelectPages": "Select the pages that contain login form and error messages",
                  "publicAccess_paSelectRoles": "Pick the roles who have access to this page",
                  "publicAccess_paSetLogin": "Set the login and password for this page",
                  "publicAccess_paSimple": "Single user protection",
                  "publicAccess_paSimpleHelp": "If you just want to setup simple protection using a single login and password",
                  "publish_contentPublishedFailedInvalid": "      %0% could not be published because these properties:  %1%  did not pass validation rules.    ",
                  "publish_contentPublishedFailedByEvent": "      %0% could not be published, due to a 3rd party extension cancelling the action.    ",
                  "publish_contentPublishedFailedByParent": "      %0% can not be published, because a parent page is not published.    ",
                  "publish_includeUnpublished": "Include unpublished child pages",
                  "publish_inProgress": "Publishing in progress - please wait...",
                  "publish_inProgressCounter": "%0% out of %1% pages have been published...",
                  "publish_nodePublish": "%0% has been published",
                  "publish_nodePublishAll": "%0% and subpages have been published",
                  "publish_publishAll": "Publish %0% and all its subpages",
                  "publish_publishHelp": "Click <em>ok</em> to publish <strong>%0%</strong> and thereby making it's content publicly available.<br/><br />      You can publish this page and all it's sub-pages by checking <em>publish all children</em> below.      ",
                  "relatedlinks_addExternal": "Add external link",
                  "relatedlinks_addInternal": "Add internal link",
                  "relatedlinks_addlink": "Add",
                  "relatedlinks_caption": "Caption",
                  "relatedlinks_internalPage": "Internal page",
                  "relatedlinks_linkurl": "URL",
                  "relatedlinks_modeDown": "Move Down",
                  "relatedlinks_modeUp": "Move Up",
                  "relatedlinks_newWindow": "Open in new window",
                  "relatedlinks_removeLink": "Remove link",
                  "rollback_currentVersion": "Current version",
                  "rollback_diffHelp": "This shows the differences between the current version and the selected version<br /><del>Red</del> text will not be shown in the selected version. , <ins>green means added</ins>",
                  "rollback_documentRolledBack": "Document has been rolled back",
                  "rollback_htmlHelp": "This displays the selected version as html, if you wish to see the difference between 2 versions at the same time, use the diff view",
                  "rollback_rollbackTo": "Rollback to",
                  "rollback_selectVersion": "Select version",
                  "rollback_view": "View",
                  "scripts_editscript": "Edit script file",
                  "sections_concierge": "Concierge",
                  "sections_content": "Content",
                  "sections_courier": "Courier",
                  "sections_developer": "Developer",
                  "sections_installer": "Umbraco Configuration Wizard",
                  "sections_media": "Media",
                  "sections_member": "Members",
                  "sections_newsletters": "Newsletters",
                  "sections_settings": "Settings",
                  "sections_statistics": "Statistics",
                  "sections_translation": "Translation",
                  "sections_users": "Users",
                  "sections_contour": "Umbraco Contour",
                  "sections_help": "Help",
                  "settings_defaulttemplate": "Default template",
                  "settings_dictionary editor egenskab": "Dictionary Key",
                  "settings_importDocumentTypeHelp": "To import a document type, find the '.udt' file on your computer by clicking the 'Browse' button and click 'Import' (you'll be asked for confirmation on the next screen)",
                  "settings_newtabname": "New Tab Title",
                  "settings_nodetype": "Nodetype",
                  "settings_objecttype": "Type",
                  "settings_stylesheet": "Stylesheet",
                  "settings_stylesheet editor egenskab": "Stylesheet property",
                  "settings_tab": "Tab",
                  "settings_tabname": "Tab Title",
                  "settings_tabs": "Tabs",
                  "settings_contentTypeEnabled": "Master Content Type enabled",
                  "settings_contentTypeUses": "This Content Type uses",
                  "settings_asAContentMasterType": "as a Master Content Type. Tabs from Master Content Types are not shown and can only be edited on the Master Content Type itself",
                  "settings_noPropertiesDefinedOnTab": "No properties defined on this tab. Click on the 'add a new property' link at the top to create a new property.",
                  "sort_sortDone": "Sorting complete.",
                  "sort_sortHelp": "Drag the different items up or down below to set how they should be arranged. Or click the column headers to sort the entire collection of items",
                  "sort_sortPleaseWait": " Please wait. Items are being sorted, this can take a while.<br/> <br/> Do not close this window during sorting",
                  "speechBubbles_contentPublishedFailedByEvent": "Publishing was cancelled by a 3rd party add-in",
                  "speechBubbles_contentTypeDublicatePropertyType": "Property type already exists",
                  "speechBubbles_contentTypePropertyTypeCreated": "Property type created",
                  "speechBubbles_contentTypePropertyTypeCreatedText": "Name: %0% <br /> DataType: %1%",
                  "speechBubbles_contentTypePropertyTypeDeleted": "Propertytype deleted",
                  "speechBubbles_contentTypeSavedHeader": "Document Type saved",
                  "speechBubbles_contentTypeTabCreated": "Tab created",
                  "speechBubbles_contentTypeTabDeleted": "Tab deleted",
                  "speechBubbles_contentTypeTabDeletedText": "Tab with id: %0% deleted",
                  "speechBubbles_cssErrorHeader": "Stylesheet not saved",
                  "speechBubbles_cssSavedHeader": "Stylesheet saved",
                  "speechBubbles_cssSavedText": "Stylesheet saved without any errors",
                  "speechBubbles_dataTypeSaved": "Datatype saved",
                  "speechBubbles_dictionaryItemSaved": "Dictionary item saved",
                  "speechBubbles_editContentPublishedFailedByParent": "Publishing failed because the parent page isn't published",
                  "speechBubbles_editContentPublishedHeader": "Content published",
                  "speechBubbles_editContentPublishedText": "and visible at the website",
                  "speechBubbles_editContentSavedHeader": "Content saved",
                  "speechBubbles_editContentSavedText": "Remember to publish to make changes visible",
                  "speechBubbles_editContentSendToPublish": "Sent For Approval",
                  "speechBubbles_editContentSendToPublishText": "Changes have been sent for approval",
                  "speechBubbles_editMediaSaved": "Media saved",
                  "speechBubbles_editMediaSavedText": "Media saved without any errors",
                  "speechBubbles_editMemberSaved": "Member saved",
                  "speechBubbles_editStylesheetPropertySaved": "Stylesheet Property Saved",
                  "speechBubbles_editStylesheetSaved": "Stylesheet saved",
                  "speechBubbles_editTemplateSaved": "Template saved",
                  "speechBubbles_editUserError": "Error saving user (check log)",
                  "speechBubbles_editUserSaved": "User Saved",
                  "speechBubbles_editUserTypeSaved": "User type saved",
                  "speechBubbles_fileErrorHeader": "File not saved",
                  "speechBubbles_fileErrorText": "file could not be saved. Please check file permissions",
                  "speechBubbles_fileSavedHeader": "File saved",
                  "speechBubbles_fileSavedText": "File saved without any errors",
                  "speechBubbles_languageSaved": "Language saved",
                  "speechBubbles_pythonErrorHeader": "Python script not saved",
                  "speechBubbles_pythonErrorText": "Python script could not be saved due to error",
                  "speechBubbles_pythonSavedHeader": "Python script saved",
                  "speechBubbles_pythonSavedText": "No errors in python script",
                  "speechBubbles_templateErrorHeader": "Template not saved",
                  "speechBubbles_templateErrorText": "Please make sure that you do not have 2 templates with the same alias",
                  "speechBubbles_templateSavedHeader": "Template saved",
                  "speechBubbles_templateSavedText": "Template saved without any errors!",
                  "speechBubbles_xsltErrorHeader": "XSLT not saved",
                  "speechBubbles_xsltErrorText": "XSLT contained an error",
                  "speechBubbles_xsltPermissionErrorText": "XSLT could not be saved, check file permissions",
                  "speechBubbles_xsltSavedHeader": "XSLT saved",
                  "speechBubbles_xsltSavedText": "No errors in XSLT",
                  "speechBubbles_contentUnpublished": "Content unpublished",
                  "speechBubbles_partialViewSavedHeader": "Partial view saved",
                  "speechBubbles_partialViewSavedText": "Partial view saved without any errors!",
                  "speechBubbles_partialViewErrorHeader": "Partial view not saved",
                  "speechBubbles_partialViewErrorText": "An error occurred saving the file.",
                  "stylesheet_aliasHelp": "Uses CSS syntax ex: h1, .redHeader, .blueTex",
                  "stylesheet_editstylesheet": "Edit stylesheet",
                  "stylesheet_editstylesheetproperty": "Edit stylesheet property",
                  "stylesheet_nameHelp": "Name to identify the style property in the rich text editor  ",
                  "stylesheet_preview": "Preview",
                  "stylesheet_styles": "Styles",
                  "template_edittemplate": "Edit template",
                  "template_insertContentArea": "Insert content area",
                  "template_insertContentAreaPlaceHolder": "Insert content area placeholder",
                  "template_insertDictionaryItem": "Insert dictionary item",
                  "template_insertMacro": "Insert Macro",
                  "template_insertPageField": "Insert umbraco page field",
                  "template_mastertemplate": "Master template",
                  "template_quickGuide": "Quick Guide to umbraco template tags",
                  "template_template": "Template",
                  "templateEditor_alternativeField": "Alternative field",
                  "templateEditor_alternativeText": "Alternative Text",
                  "templateEditor_casing": "Casing",
                  "templateEditor_encoding": "Encoding",
                  "templateEditor_chooseField": "Choose field",
                  "templateEditor_convertLineBreaks": "Convert Linebreaks",
                  "templateEditor_convertLineBreaksHelp": "Replaces linebreaks with html-tag &lt;br&gt;",
                  "templateEditor_customFields": "Custom Fields",
                  "templateEditor_dateOnly": "Yes, Date only",
                  "templateEditor_formatAsDate": "Format as date",
                  "templateEditor_htmlEncode": "HTML encode",
                  "templateEditor_htmlEncodeHelp": "Will replace special characters by their HTML equivalent.",
                  "templateEditor_insertedAfter": "Will be inserted after the field value",
                  "templateEditor_insertedBefore": "Will be inserted before the field value",
                  "templateEditor_lowercase": "Lowercase",
                  "templateEditor_none": "None",
                  "templateEditor_postContent": "Insert after field",
                  "templateEditor_preContent": "Insert before field",
                  "templateEditor_recursive": "Recursive",
                  "templateEditor_removeParagraph": "Remove Paragraph tags",
                  "templateEditor_removeParagraphHelp": "Will remove any &lt;P&gt; in the beginning and end of the text",
                  "templateEditor_standardFields": "Standard Fields",
                  "templateEditor_uppercase": "Uppercase",
                  "templateEditor_urlEncode": "URL encode",
                  "templateEditor_urlEncodeHelp": "Will format special characters in URLs",
                  "templateEditor_usedIfAllEmpty": "Will only be used when the field values above are empty",
                  "templateEditor_usedIfEmpty": "This field will only be used if the primary field is empty",
                  "templateEditor_withTime": "Yes, with time. Seperator: ",
                  "translation_assignedTasks": "Tasks assigned to you",
                  "translation_assignedTasksHelp": " The list below shows translation tasks <strong>assigned to you</strong>. To see a detailed view including comments, click on 'Details' or just the page name.      You can also download the page as XML directly by clicking the 'Download Xml' link. <br/>     To close a translation task, please go to the Details view and click the 'Close' button.    ",
                  "translation_closeTask": "close task",
                  "translation_details": "Translation details",
                  "translation_downloadAllAsXml": "Download all translation tasks as xml",
                  "translation_downloadTaskAsXml": "Download xml",
                  "translation_DownloadXmlDTD": "Download xml DTD",
                  "translation_fields": "Fields",
                  "translation_includeSubpages": "Include subpages",
                  "translation_mailBody": "      Hi %0%      This is an automated mail to inform you that the document '%1%'      has been requested for translation into '%5%' by %2%.      Go to http://%3%/translation/details.aspx?id=%4% to edit.      Or log into umbraco to get an overview of your translation tasks      http://%3%      Have a nice day!      Cheers from the umbraco robot    ",
                  "translation_mailSubject": "[%0%] Translation task for %1%",
                  "translation_noTranslators": "No translator users found. Please create a translator user before you start sending content to translation",
                  "translation_ownedTasks": "Tasks created by you",
                  "translation_ownedTasksHelp": " The list below shows pages <strong>created by you</strong>. To see a detailed view including comments,      click on 'Details' or just the page name. You can also download the page as XML directly by clicking the 'Download Xml' link.      To close a translation task, please go to the Details view and click the 'Close' button.    ",
                  "translation_pageHasBeenSendToTranslation": "The page '%0%' has been send to translation",
                  "translation_sendToTranslate": "Send the page '%0%' to translation",
                  "translation_taskAssignedBy": "Assigned by",
                  "translation_taskOpened": "Task opened",
                  "translation_totalWords": "Total words",
                  "translation_translateTo": "Translate to",
                  "translation_translationDone": "Translation completed.",
                  "translation_translationDoneHelp": "You can preview the pages, you've just translated, by clicking below. If the original page is found, you will get a comparison of the 2 pages.",
                  "translation_translationFailed": "Translation failed, the xml file might be corrupt",
                  "translation_translationOptions": "Translation options",
                  "translation_translator": "Translator",
                  "translation_uploadTranslationXml": "Upload translation xml",
                  "treeHeaders_cacheBrowser": "Cache Browser",
                  "treeHeaders_contentRecycleBin": "Recycle Bin",
                  "treeHeaders_createdPackages": "Created packages",
                  "treeHeaders_datatype": "Data Types",
                  "treeHeaders_dictionary": "Dictionary",
                  "treeHeaders_installedPackages": "Installed packages",
                  "treeHeaders_installSkin": "Install skin",
                  "treeHeaders_installStarterKit": "Install starter kit",
                  "treeHeaders_languages": "Languages",
                  "treeHeaders_localPackage": "Install local package",
                  "treeHeaders_macros": "Macros",
                  "treeHeaders_mediaTypes": "Media Types",
                  "treeHeaders_member": "Members",
                  "treeHeaders_memberGroup": "Member Groups",
                  "treeHeaders_memberRoles": "Roles",
                  "treeHeaders_memberType": "Member Types",
                  "treeHeaders_nodeTypes": "Document Types",
                  "treeHeaders_packager": "Packages",
                  "treeHeaders_packages": "Packages",
                  "treeHeaders_python": "Python Files",
                  "treeHeaders_repositories": "Install from repository",
                  "treeHeaders_runway": "Install Runway",
                  "treeHeaders_runwayModules": "Runway modules",
                  "treeHeaders_scripting": "Scripting Files",
                  "treeHeaders_scripts": "Scripts",
                  "treeHeaders_stylesheets": "Stylesheets",
                  "treeHeaders_templates": "Templates",
                  "treeHeaders_xslt": "XSLT Files",
                  "update_updateAvailable": "New update ready",
                  "update_updateDownloadText": "%0% is ready, click here for download",
                  "update_updateNoServer": "No connection to server",
                  "update_updateNoServerError": "Error checking for update. Please review trace-stack for further information",
                  "user_administrators": "Administrator",
                  "user_categoryField": "Category field",
                  "user_changePassword": "Change Your Password",
                  "user_newPassword": "Change Your Password",
                  "user_confirmNewPassword": "Confirm new password",
                  "user_changePasswordDescription": "You can change your password for accessing the Umbraco Back Office by filling out the form below and click the 'Change Password' button",
                  "user_contentChannel": "Content Channel",
                  "user_descriptionField": "Description field",
                  "user_disabled": "Disable User",
                  "user_documentType": "Document Type",
                  "user_editors": "Editor",
                  "user_excerptField": "Excerpt field",
                  "user_language": "Language",
                  "user_loginname": "Login",
                  "user_mediastartnode": "Start Node in Media Library",
                  "user_modules": "Sections",
                  "user_noConsole": "Disable Umbraco Access",
                  "user_password": "Password",
                  "user_resetPassword": "Reset password",
                  "user_passwordChanged": "Your password has been changed!",
                  "user_passwordConfirm": "Please confirm the new password",
                  "user_passwordEnterNew": "Enter your new password",
                  "user_passwordIsBlank": "Your new password cannot be blank!",
                  "user_passwordCurrent": "Current password",
                  "user_passwordInvalid": "Invalid current password",
                  "user_passwordIsDifferent": "There was a difference between the new password and the confirmed password. Please try again!",
                  "user_passwordMismatch": "The confirmed password doesn't match the new password!",
                  "user_permissionReplaceChildren": "Replace child node permssions",
                  "user_permissionSelectedPages": "You are currently modifying permissions for the pages:",
                  "user_permissionSelectPages": "Select pages to modify their permissions",
                  "user_searchAllChildren": "Search all children",
                  "user_startnode": "Start Node in Content",
                  "user_username": "Username",
                  "user_userPermissions": "User permissions",
                  "user_usertype": "User type",
                  "user_userTypes": "User types",
                  "user_writer": "Writer",
                  "user_yourProfile": "Your profile",
                  "user_yourHistory": "Your recent history",
                  "user_sessionExpires": "Session expires in"
              }, null];
          }
      }

      return {
          register: function() {
              $httpBackend
                  .whenGET(mocksUtils.urlRegex('js/language.aspx'))
                  .respond(getLanguageResource);
          }
      };
  }]);
/**
* @ngdoc service
* @name umbraco.mocks.mediaHelperService
* @description A helper object used for dealing with media items
**/
function mediaHelper(umbRequestHelper) {
    return {
        /**
         * @ngdoc function
         * @name umbraco.services.mediaHelper#getImagePropertyValue
         * @methodOf umbraco.services.mediaHelper
         * @function    
         *
         * @description
         * Returns the file path associated with the media property if there is one
         * 
         * @param {object} options Options object
         * @param {object} options.mediaModel The media object to retrieve the image path from
         * @param {object} options.imageOnly Optional, if true then will only return a path if the media item is an image
         */
        getMediaPropertyValue: function (options) {
            return "assets/img/mocks/big-image.jpg";
        },
        
        /**
         * @ngdoc function
         * @name umbraco.services.mediaHelper#getImagePropertyValue
         * @methodOf umbraco.services.mediaHelper
         * @function    
         *
         * @description
         * Returns the actual image path associated with the image property if there is one
         * 
         * @param {object} options Options object
         * @param {object} options.imageModel The media object to retrieve the image path from
         */
        getImagePropertyValue: function (options) {
            return "assets/img/mocks/big-image.jpg";
        },
        /**
         * @ngdoc function
         * @name umbraco.services.mediaHelper#getThumbnail
         * @methodOf umbraco.services.mediaHelper
         * @function    
         *
         * @description
         * formats the display model used to display the content to the model used to save the content
         * 
         * @param {object} options Options object
         * @param {object} options.imageModel The media object to retrieve the image path from
         */
        getThumbnail: function (options) {

            if (!options || !options.imageModel) {
                throw "The options objet does not contain the required parameters: imageModel";
            }

            var imagePropVal = this.getImagePropertyValue(options);
            if (imagePropVal !== "") {
                return this.getThumbnailFromPath(imagePropVal);
            }
            return "";
        },

        /**
         * @ngdoc function
         * @name umbraco.services.mediaHelper#scaleToMaxSize
         * @methodOf umbraco.services.mediaHelper
         * @function    
         *
         * @description
         * Finds the corrct max width and max height, given maximum dimensions and keeping aspect ratios
         * 
         * @param {number} maxSize Maximum width & height
         * @param {number} width Current width
         * @param {number} height Current height
         */
        scaleToMaxSize: function (maxSize, width, height) {
            var retval = { width: width, height: height };

            var maxWidth = maxSize; // Max width for the image
            var maxHeight = maxSize;    // Max height for the image
            var ratio = 0;  // Used for aspect ratio

            // Check if the current width is larger than the max
            if (width > maxWidth) {
                ratio = maxWidth / width;   // get ratio for scaling image

                retval.width = maxWidth;
                retval.height = height * ratio;

                height = height * ratio;    // Reset height to match scaled image
                width = width * ratio;    // Reset width to match scaled image
            }

            // Check if current height is larger than max
            if (height > maxHeight) {
                ratio = maxHeight / height; // get ratio for scaling image

                retval.height = maxHeight;
                retval.width = width * ratio;
                width = width * ratio;    // Reset width to match scaled image
            }

            return retval;
        },

        /**
         * @ngdoc function
         * @name umbraco.services.mediaHelper#getThumbnailFromPath
         * @methodOf umbraco.services.mediaHelper
         * @function    
         *
         * @description
         * Returns the path to the thumbnail version of a given media library image path
         * 
         * @param {string} imagePath Image path, ex: /media/1234/my-image.jpg
         */
        getThumbnailFromPath: function (imagePath) {
            return "assets/img/mocks/big-thumb.jpg";
        },

        /**
         * @ngdoc function
         * @name umbraco.services.mediaHelper#detectIfImageByExtension
         * @methodOf umbraco.services.mediaHelper
         * @function    
         *
         * @description
         * Returns true/false, indicating if the given path has an allowed image extension
         * 
         * @param {string} imagePath Image path, ex: /media/1234/my-image.jpg
         */
        detectIfImageByExtension: function (imagePath) {
            var lowered = imagePath.toLowerCase();
            var ext = lowered.substr(lowered.lastIndexOf(".") + 1);
            return ("," + Umbraco.Sys.ServerVariables.umbracoSettings.imageFileTypes + ",").indexOf("," + ext + ",") !== -1;
        }
    };
}
angular.module('umbraco.mocks').factory('mediaHelper', mediaHelper);
angular.module('umbraco.mocks').
  factory('utilMocks', ['$httpBackend', 'mocksUtils', function ($httpBackend, mocksUtils) {
      'use strict';
      
      function getUpdateCheck(status, data, headers) {
          //check for existence of a cookie so we can do login/logout in the belle app (ignore for tests).
          if (!mocksUtils.checkAuth()) {
              return [401, null, null];
          }
          else {
              return [200, null, null];
          }
      }

      return {
          register: function() {
              $httpBackend
                  .whenGET(mocksUtils.urlRegex('/umbraco/Api/UpdateCheck/GetCheck'))
                  .respond(getUpdateCheck);
          }
      };
  }]);

})();