#include "class-factory.h"
#include "pch.h"
#include "register-helper.h"

HINSTANCE module = nullptr;
long g_dllRefCounter = 0;

// {FAB3CC2C-6045-4A26-96C1-9FE2B891195A}
static const CLSID CLSID_VkloudMenuExt = {0xfab3cc2c, 0x6045, 0x4a26, {0x96, 0xc1, 0x9f, 0xe2, 0xb8, 0x91, 0x19, 0x5a}};

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        module = hModule;
        break;

    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }

    return TRUE;
}

STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, void** ppv)
{
    HRESULT hr = CLASS_E_CLASSNOTAVAILABLE;

    if (IsEqualCLSID(CLSID_VkloudMenuExt, rclsid))
    {
        hr = E_OUTOFMEMORY;

        auto factory = new ClassFactory();
        hr = factory->QueryInterface(riid, ppv);
        factory->Release();
    }

    return hr;
}

STDAPI DllCanUnloadNow()
{
    return S_OK;
}

STDAPI DllRegisterServer()
{
    HRESULT result;

    wchar_t szModule[MAX_PATH];
    if (GetModuleFileName(module, szModule, ARRAYSIZE(szModule)) == 0)
    {
        result = HRESULT_FROM_WIN32(GetLastError());
        return result;
    }

    // Register the component.
    result = utils::RegisterInprocServer(szModule, CLSID_VkloudMenuExt,
                                         L"CppShellExtContextMenuHandler.FileContextMenuExt Class", L"Apartment");
    if (SUCCEEDED(result))
    {
        // Register the context menu handler. The context menu handler is
        // associated with the .cpp file class.
        result = utils::RegisterShellExtContextMenuHandler(L".txt", CLSID_VkloudMenuExt,
                                                           L"CppShellExtContextMenuHandler.FileContextMenuExt");
    }

    return result;
}

STDAPI DllUnregisterServer()
{
    HRESULT result = S_OK;

    wchar_t szModule[MAX_PATH];
    if (GetModuleFileName(module, szModule, ARRAYSIZE(szModule)) == 0)
    {
        result = HRESULT_FROM_WIN32(GetLastError());
        return result;
    }

    // Unregister the component.
    result = utils::UnregisterInprocServer(CLSID_VkloudMenuExt);
    if (SUCCEEDED(result))
    {
        // Unregister the context menu handler.
        result = utils::UnregisterShellExtContextMenuHandler(L".txt", CLSID_VkloudMenuExt);
    }

    return result;
}
