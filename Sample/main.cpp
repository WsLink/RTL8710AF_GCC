extern "C" {
	#include "diag.h"
	#include "gpio_api.h"

	// 引用这个变量可导致 lib_platform.a(startup.o) 被链接，从而确保整体成功
	#if !defined (__ICCARM__)
		extern u8 RAM_IMG1_VALID_PATTEN[];
		void *tmp = RAM_IMG1_VALID_PATTEN;
	#endif
}

int main(void)
{
    gpio_t gpio_led;

	DiagPrintf("万家灯火，无声物联！\r\n");

    gpio_init(&gpio_led, PC_5);
    gpio_dir(&gpio_led, PIN_OUTPUT);
    gpio_mode(&gpio_led, PullNone);
    while(1){
        gpio_write(&gpio_led, !gpio_read(&gpio_led));
        //RtlMsleepOS(1000);
		volatile int n = 100000;
		while(n--);
    }

	return 0;
}
