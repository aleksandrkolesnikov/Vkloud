#pragma once

#include <Windows.h>

namespace utils
{

HRESULT RegisterInprocServer(LPCWSTR module, const CLSID& id, PCWSTR friendlyNAme, PCWSTR threadModel);
HRESULT UnregisterInprocServer(const CLSID& clsid);
HRESULT RegisterShellExtContextMenuHandler(PCWSTR fileType, const CLSID& id, PCWSTR friendlyName);
HRESULT UnregisterShellExtContextMenuHandler(PCWSTR fileType, const CLSID& id);

} // namespace utils
