#!/bin/bash

dotnet Bookstore.Auth.dll &
dotnet Bookstore.WebHost.dll
wait
