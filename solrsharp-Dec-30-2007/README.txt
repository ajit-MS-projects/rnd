# Licensed to the Apache Software Foundation (ASF) under one or more
# contributor license agreements.  See the NOTICE file distributed with
# this work for additional information regarding copyright ownership.
# The ASF licenses this file to You under the Apache License, Version 2.0
# (the "License"); you may not use this file except in compliance with
# the License.  You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.


Welcome to SolrSharp -- a C# API implementation against the Solr search server
------------------------------------------------------------------------------

Apache Solr is a search server based on the Apache Lucene search library. 

SolrSharp is a client API written in C# using the .Net framework to interact
with Apache Solr.

For a complete description of the Solr project, team composition, source
code repositories, and other details, please see the Solr web site at
http://lucene.apache.org/solr.html.


Requirements
------------

SolrSharp requires the .Net Framework, version 2.0 available from Microsoft. 
For current versions and download links, please see http://msdn.microsoft.com.

SolrSharp requires the Solr search server.  For current versions and download 
links, please see http://lucene.apache.org/solr.html.

SolrSharp was created by Jeff Rodenburg using Microsoft Visual Studio 2005. 
You may use any development environment that supports compilation of .Net 
Framework 2.0 libraries to use the SolrSharp library in your own project.

Send questions to jeff-dot-rodenburg-at-g-mail-dot-com.



Getting Started
---------------
1. Get a solr webapp instance up and running.
2. See the "example" directory for an example SolrSharp implementation.


Files Included In SolrSharp
---------------------------
example/
  A self-contained example, designed to work with the example Solr instance.

docs/api/index.html
  The SolrSharp MSDN-style API documentation.

src/
  The SolrSharp source code.



Instructions for Building SolrSharp from Source
-------------------------------------------------

1. SolrSharp includes Microsoft Visual Studio 2005 project and solution
   files.  Open the SolrSharp solution file and run "Build Solution".

   NOTE:
   This is a demand-driven project.  If you have a need to build SolrSharp outside
   of Microsoft Visual Studio 2005, please let me and the community know.
